using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic1 : AbstractRoomLogic
{

    bool screwdriver;
    bool valve;
    bool telephoneOut;
    bool telephoneDismantle;
    bool keyTook;
    bool batteryTook;

    public AudioSource roomMusic;

    public enum doorState
    {
        initial,
        locked,
        unlocked
    }

    doorState actualDoorState;

    public override void InteractionHappened(string interactionID, bool important = true)
    {
        base.InteractionHappened(interactionID, important);
        switch(interactionID)
        {
            case "Window_dark":
                ShowTextUI.instance.ShowMainText("Seeing the outside fills you with unease.");
                break;
            case "Window_light":
                ShowTextUI.instance.ShowMainText("The stars seem especially pleased today.");
                break;
            case "window-blind":
                ShowTextUI.instance.ShowMainText("You can hear a slight hum of the world on the other side.");
                break;
            case "toilet":
                if (valve)
                {
                    ShowTextUI.instance.ShowMainText("You'd rather not go through that again.");
                    return;
                }

                ShowTextUI.instance.ShowMainText("A rather dirty toilet towers above you menacingly.", () => {
                    ShowTextUI.instance.ShowChoiceText("There seems to be something inside. Reach in?",
                        () => {
                            ShowTextUI.instance.ShowMainText("After a small battle with the toilet, you manage to retrieve a grimy valve.", () => {
                                InventoryManager.instance.AddItemToInventory(inventoryItems.valve);
                                valve = true;
                            });
                        },
                        () => { ShowTextUI.instance.ShowMainText("With a slight, temporary, relief you decide not to put your hands inside a toilet."); });
                });
                break;

            case "bookcase":
                if(!screwdriver)
                {
                    ShowTextUI.instance.ShowMainText("A familiar-looking bookshelf, full of useful things.", () => {
                        ShowTextUI.instance.ShowMainText("As you dart over the things tastefully put on the shelves… ", () => {
                            ShowTextUI.instance.ShowMainText("… you notice a screwdriver that piques your interest!", () => {
                                ShowTextUI.instance.ShowChoiceText("You feel a strange attraction to it, would you like to take it?",
                                    () => {
                                        ShowTextUI.instance.ShowMainText("As you hold the screwdriver in your tiny hands, a short burst of confidence fills your heart.", () => {
                                            InventoryManager.instance.AddItemToInventory(inventoryItems.screwdriver);
                                            screwdriver = true;
                                        });
                                    },
                                    () => {
                                        ShowTextUI.instance.ShowMainText("You decide not to accept the company of the screwdriver.");
                                 });
                            });
                        });
                    });
                }
                else
                {
                    if(SpellingWordsManager.instance.ContainsWord(spellingWords.wooden) && InventoryManager.instance.selectedItem != inventoryItems.valve)
                    {
                        ShowTextUI.instance.ShowMainText("Puzzled, you stare at a hexagonal piece of metal sticking from the side of the bookshelf.", () => {
                            ShowTextUI.instance.ShowMainText("You feel confident that it was not there before.");
                        });
                    }
                    else if(SpellingWordsManager.instance.ContainsWord(spellingWords.wooden) && InventoryManager.instance.selectedItem == inventoryItems.valve)
                    {
                        ShowTextUI.instance.ShowMainText("With a satisfying click, the valve fits perfectly in.", () => {
                            CutsceneUI.instance.ShowCutScene(0f, () => {
                                SyncManager.instance.InvokeTurnOn("bookcase_opened");
                                SyncManager.instance.InvokeTurnOff("bookcase");
                                InventoryManager.instance.RemoveItemFromInventory(inventoryItems.valve);
                                ShowTextUI.instance.ShowMainText("While panting slightly from exhaustion, you stare at a glistening metal door in front of you.");
                                AnalyticsClass.SendCustomEvent("SecretDoorFound",
                                    new Dictionary<string, object>() {
                                        {"time", Time.unscaledTime }
                                    });
                            });
                        });
                    }
                    else
                    {
                        ShowTextUI.instance.ShowMainText("You skim through all the items on the bookshelf, however, nothing piques your interest.");
                    }
                }
                break;
            case "door":
                switch (actualDoorState)
                {
                    case doorState.initial:
                        if(OnboardingManager.instance)
                            OnboardingManager.instance.DoorInteracted();
                        ShowTextUI.instance.ShowMainText("You try twisting the handle… but the door refuses to open.", () => {
                            roomMusic.Play();
                            ShowTextUI.instance.ShowMainText("A sudden chill runs down your spine.", () => {
                                actualDoorState = doorState.locked; 
                                //OnboardingManager.instance.ShowSpace(true);
                                BookManager.instance.ChangeIncrement(1);
                                TimersManager.instance.AddTimer(15f, () => { 
                                    BookManager.instance.ChangeIncrement(2); }, true);
                                });
                        });
                        break;
                    case doorState.locked:
                        if(IsItemSelected(inventoryItems.key))
                        {
                            ShowTextUI.instance.ShowMainText("The key fits snugly in the keyhole.", () => {
                                ShowTextUI.instance.ShowMainText("With a twist and a satisfying crackle, the door unlocks.", () => {
                                    actualDoorState = doorState.unlocked;
                                    InventoryManager.instance.RemoveItemFromInventory(inventoryItems.key);
                                    AnalyticsClass.SendCustomEvent("MainDoorsUnlocked",
                                        new Dictionary<string, object>() {
                                            {"time", Time.unscaledTime }
                                        });
                                });
                            });
                        }
                        else
                            ShowTextUI.instance.ShowMainText("Try as you might, the door remains firmly locked.");
                        break;
                    case doorState.unlocked:
                        ShowTextUI.instance.ShowMainText("The warm wooden door welcomes you.", () => {
                            ShowTextUI.instance.ShowChoiceText("Enter the door?",
                                () => {
                                    ShowTextUI.instance.ShowMainText("With an accepting wooden sound, the door opens wide.", () => {
                                        LeaveRoom();
                                    });
                                },
                                () => { ShowTextUI.instance.ShowMainText("You feel an unexplainable urge to stay for a bit longer."); });
                        });
                        break;
                    default:
                        break;
                }
                break;
            case "bookcase_opened":
                ShowTextUI.instance.ShowMainText("Although imposing, the thick metal door is innocuously unlocked.", () => {
                    ShowTextUI.instance.ShowChoiceText("Go through the door?",
                        () => {
                            ShowTextUI.instance.ShowMainText("With a sharp metallic screech, the door welcomes you.", () => {
                                LeaveRoom();
                            });
                        },
                        () => { ShowTextUI.instance.ShowMainText("You decide to stay a little longer."); });
                });
                break;
            case "rotary-telephone":
                if(IsItemSelected(inventoryItems.screwdriver) && !keyTook && IsWordSelected(spellingWords.rotary_telephones))
                {
                    ShowTextUI.instance.ShowMainText("You gently take the rotary telephone apart.", () => {
                        ShowTextUI.instance.ShowMainText("It was strangely relaxing.", () => {
                            ShowTextUI.instance.ShowMainText("Oh, and also you found a key inside. You carefully slid it into your pockets.", () => { 
                                keyTook = true;
                                InventoryManager.instance.AddItemToInventory(inventoryItems.key);
                            });
                        });
                    });
                }
                else
                {
                    if(keyTook && !IsWordSelected(spellingWords.rotary_telephones))
                    {
                        ShowTextUI.instance.ShowMainText("The phone feels lighter for some reason. Moving it around you feel a click-clack, indicating an object trapped inside.", () => {
                            ShowTextUI.instance.ShowChoiceText("Smash the phone open?",
                                () => { ShowTextUI.instance.ShowMainText("That would just hurt you more than the phone."); },
                                () => { ShowTextUI.instance.ShowMainText("You allow the idea to sizzle out of your mind, as you gently put the old telephone back on the soft carpet."); });
                        });                        
                    }
                    else
                    {
                        ShowTextUI.instance.ShowMainText("An old unused rotary telephone. You feel a remorseful kinship with it.");
                    }
                }

                break;
            case "bed":
                if(IsWordSelected(spellingWords.bright) && !telephoneOut)
                {
                    ShowTextUI.instance.ShowMainText("While looking longingly at the bed, you notice a slight reflection of the starry skies on something under the bed.", () => {
                        ShowTextUI.instance.ShowChoiceText("Reach under the bed?",
                                    () => {
                                        telephoneOut = true;
                                        CutsceneUI.instance.ShowCutScene(.5f, () => {
                                            SyncManager.instance.InvokeTurnOn("rotary-telephone");
                                            ShowTextUI.instance.ShowMainText("You reach into the barely lit darkness until you take a firm grasp of the thing beneath the bed.");
                                        });
                                    },
                                    () => { ShowTextUI.instance.ShowMainText("Your heart sinks while your imagination runs wild on all the possible denizens of the under-bed."); });
                    });
                }
                else if(IsWordSelected(spellingWords.sleepy))
                {
                    ShowTextUI.instance.ShowChoiceText("Go to sleep?",
                                    () => { CutsceneUI.instance.ShowCutScene(.5f, 
                                        () => { 
                                                if (!batteryTook)
                                                {
                                                    CutsceneUI.instance.ShowCutScene(2f, () =>
                                                    {
                                                        ShowTextUI.instance.ShowMainText("You lie down, and you find it difficult to sleep as something is beneath the bed-sheets. ", () =>
                                                        {
                                                            ShowTextUI.instance.ShowMainText("After a short search, you find a tiny battery cell.");

                                                            InventoryManager.instance.AddItemToInventory(inventoryItems.battery);
                                                            batteryTook = true;
                                                        });
                                                    });
                                                }
                                                else
                                                {
                                                    ShowTextUI.instance.ShowMainText("You lie down, in an attempt to fall asleep. However, very unpleasant memories flood your mind.", () => {
                                                        ShowTextUI.instance.ShowMainText("You decide to get back up.");
                                                    });
                                                }
                                            });
                                        },
                                        () => { ShowTextUI.instance.ShowMainText("You walk away, rejecting the tempting offer."); });
                }
                else
                {
                    ShowTextUI.instance.ShowChoiceText("Go to sleep?",
                                    () => { ShowTextUI.instance.ShowMainText("A comfortable-looking bed. However, you don’t feel very sleepy."); },
                                    () => { ShowTextUI.instance.ShowMainText("You walk away, rejecting the tempting offer."); });
                }
                break;
            case "tv":
                ShowTextUI.instance.ShowMainText("You joyfully run your hands across the CRT television.", () => {
                    ShowTextUI.instance.ShowMainText("Static buzzing between your fingers and the dust particles captured on the top of the glass.");
                });
                break;
            case "sofa":
                ShowTextUI.instance.ShowMainText("Warm memories flood your mind. Just looking at the sofa makes you comfortable.");
                break;
            case "pile-of-rocks":
                ShowTextUI.instance.ShowMainText("A very out-of-place pile of rocks. They are surprisingly cold to the touch.");
                break;
            
            default:
                break;
        }
    }

    void LeaveRoom()
    {
        SceneChangeManager.instance.ChangeScene("Hallway");
    }

    public override void Init()
    {
        for (int i = 0; i < SpellingWordsManager.instance.selectedWordsList.Count; i++)
            WordChanged(SpellingWordsManager.instance.selectedWordsList[i], false);
    }

    public override void WordChanged(spellingWords word, bool important = true)
    {
        base.WordChanged(word, important);
        switch (word)
        {
            case spellingWords.lonely:
                SyncManager.instance.InvokeTurnOff("Window_light");
                SyncManager.instance.InvokeTurnOn("Window_dark");
                break;
            case spellingWords.bright:
                SyncManager.instance.InvokeTurnOn("Window_light");
                SyncManager.instance.InvokeTurnOff("Window_dark");
                break;
            case spellingWords.fluffy:
                PlayerMovement.instance.animator.SetBool("Sleepy", false);
                break;
            case spellingWords.furry:
                PlayerMovement.instance.animator.SetBool("Sleepy", false);
                break;
            case spellingWords.sleepy:
                PlayerMovement.instance.animator.SetBool("Sleepy", true);
                break;
            case spellingWords.only:
                break;
            case spellingWords.wooden:
                break;
            case spellingWords.metal_safes:
                break;
            case spellingWords.soap_bars:
                break;
            case spellingWords.rotary_telephones:
                break;
            case spellingWords.sofa:
                SyncManager.instance.InvokeTurnOn("sofa");
                SyncManager.instance.InvokeTurnOff("pile-of-rocks");
                SyncManager.instance.InvokeTurnOff("toilet");
                break;
            case spellingWords.pile_of_rocks:
                SyncManager.instance.InvokeTurnOff("sofa");
                SyncManager.instance.InvokeTurnOn("pile-of-rocks");
                SyncManager.instance.InvokeTurnOff("toilet");
                break;
            case spellingWords.toilet:
                SyncManager.instance.InvokeTurnOff("sofa");
                SyncManager.instance.InvokeTurnOff("pile-of-rocks");
                SyncManager.instance.InvokeTurnOn("toilet");
                break;
            default:
                break;
        }
    }
}
