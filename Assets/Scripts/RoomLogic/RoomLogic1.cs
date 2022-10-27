using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic1 : AbstractRoomLogic
{

    bool screwdriver;
    bool valve;

    public enum doorState
    {
        initial,
        locked,
        unlocked
    }

    doorState actualDoorState;
    public override void InteractionHappened(string interactionID)
    {
        switch(interactionID)
        {
            case "Window_dark":
                ShowTextUI.instance.ShowMainText("Seeing the outside fills you with unease.");
                break;
            case "Window_light":
                ShowTextUI.instance.ShowMainText("The stars seem especially pleased today.");
                break;
            case "window-blind":
                ShowTextUI.instance.ShowMainText("You can hear a slight hum of the world on the other side of the window.");
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
                        ShowTextUI.instance.ShowMainText("As you dart over the things tastefully put on the shelves, you notice a screwdriver piques your interest.", () => {
                            ShowTextUI.instance.ShowChoiceText("You feel a strange attraction to it, would you like to take it?",
                                () => {
                                    ShowTextUI.instance.ShowMainText("As you hold the screwdriver in your tiny hands, a short burst of confidence fills your heart.", () => {
                                        InventoryManager.instance.AddItemToInventory(inventoryItems.screwdriver);
                                        screwdriver = true;
                                    });
                                },
                                () => { ShowTextUI.instance.ShowMainText("With a slight, temporary, relief you decide not to put your hands inside a toilet."); });
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
                            ShowTextUI.instance.ShowMainText("While panting slightly from exhaustion, you stare at a glistening metal door in front of you.", () => { Debug.Log("Otvor vrata asi"); });
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
                        ShowTextUI.instance.ShowMainText("You try twisting the handle… but the door refuses to open.", () => {
                            ShowTextUI.instance.ShowMainText("A sudden chill runs down your spine.", () => { actualDoorState = doorState.locked; });
                        });
                        break;
                    case doorState.locked:
                        if(IsItemSelected(inventoryItems.key))
                        {
                            ShowTextUI.instance.ShowMainText("The key fits snugly in the keyhole.", () => {
                                ShowTextUI.instance.ShowMainText("With a twist and a satisfying crackle, the door unlocks.", () => {Debug.Log("Treba otvori dvere no."); actualDoorState = doorState.unlocked; });
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
                                        Debug.Log("Leave room i guess");
                                    });
                                },
                                () => { ShowTextUI.instance.ShowMainText("You feel an unexplainable urge to stay for a bit longer."); });
                        });
                        break;
                    default:
                        break;
                }
                break;
            case "bed2":
                break;
            case "bed3":
                break;
            case "bed":
                ShowTextUI.instance.ShowChoiceText("Go to sleep?",
                                () => { ShowTextUI.instance.ShowMainText("You lie down, in an attempt to fall asleep. However, very unpleasant memories flood your mind. You decide to get back up.)");},
                                () => { ShowTextUI.instance.ShowMainText("You walk away, rejecting the tempting offer."); });
                break;
            case "tv":
                ShowTextUI.instance.ShowMainText("You joyfully run your hands across the CRT television, static buzzing between your fingers and the dust particles captured on the top of the glass.");
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

    public override void Init()
    {
        for (int i = 0; i < SpellingWordsManager.instance.selectedWordsList.Count; i++)
            WordChanged(SpellingWordsManager.instance.selectedWordsList[i]);
    }

    public override void WordChanged(spellingWords word)
    {
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
                break;
            case spellingWords.furry:
                break;
            case spellingWords.sleepy:
                break;
            case spellingWords.only:
                break;
            case spellingWords.wooden:
                break;
            case spellingWords.metal_safes:
                break;
            case spellingWords.dog_food:
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

    public void ShowSomeText(string text)
    {
        Debug.Log("Show: " + text);
    }
}
