using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomLogic2 : AbstractRoomLogic
{
    bool hydrant1;
    bool hydrant2;
    bool hydrant3;

    public override void Init()
    {
        for (int i = 0; i < SpellingWordsManager.instance.selectedWordsList.Count; i++)
            WordChanged(SpellingWordsManager.instance.selectedWordsList[i]);

        BookManager.instance.UnlockNewChapter(2);

        if (PlayerMovement.hasFlashlight)
            SyncManager.instance.InvokeTurnOff("ScareArea");
    }

    public override void InteractionHappened(string interactionID, bool important = true)
    {
        base.InteractionHappened(interactionID, important);

        switch (interactionID)
        {
            case "door_room":
                ShowTextUI.instance.ShowMainText("The warm wooden door welcomes you.", () => {
                    ShowTextUI.instance.ShowChoiceText("Enter the door?",
                        () => {
                            ShowTextUI.instance.ShowMainText("With an accepting wooden sound, the door opens wide.", () => {
                                SceneChangeManager.instance.ChangeScene("Game1");
                            });
                        },
                        () => { ShowTextUI.instance.ShowMainText("You prefer not to."); });
                });
                break;
            case "plumbing":
                ShowTextUI.instance.ShowMainText("Set of pipes stained yellow by a time gone by. Faint guttural noises can be heard from within.");
                break;
            case "hydrant-1":
                if(hydrant1)
                {
                    ShowTextUI.instance.ShowMainText("You don’t feel it's necessary to open it again.");
                    break;
                }
                ShowTextUI.instance.ShowMainText("A closed hydrant box. Who knows what treasures lie within?", () => {
                    ShowTextUI.instance.ShowChoiceText("Open?",
                        () => {
                            ShowTextUI.instance.ShowMainText("You search the hydrant with your cute tiny eyes, but there’s nothing but a hydrant hose.", () => {
                                ShowTextUI.instance.ShowMainText("What did you expect, silly?");
                                hydrant1 = true;
                            });
                        },
                        () => { ShowTextUI.instance.ShowMainText("You prefer not to."); });
                });
                break;
            case "hydrant-2":
                if (hydrant2)
                {
                    ShowTextUI.instance.ShowMainText("You don’t feel it's necessary to open it again.");
                    break;
                }
                ShowTextUI.instance.ShowMainText("A closed hydrant box. Who knows what treasures lie within?", () => {
                    ShowTextUI.instance.ShowChoiceText("Open?",
                        () => {
                            ShowTextUI.instance.ShowMainText("Your ears perk up as you notice a small shiny object inside the metal box.", () => {
                                ShowTextUI.instance.ShowMainText("Key obtained!");
                                hydrant2 = true;
                                InventoryManager.instance.AddItemToInventory(inventoryItems.hall_key);
                            });
                        },
                        () => { ShowTextUI.instance.ShowMainText("You prefer not to."); });
                });
                break;
            case "hydrant-3":
                if (hydrant3)
                {
                    ShowTextUI.instance.ShowMainText("You don’t feel it's necessary to open it again.");
                    break;
                }
                ShowTextUI.instance.ShowMainText("A closed hydrant box. Who knows what treasures lie within?", () => {
                    ShowTextUI.instance.ShowChoiceText("Open?",
                        () => {
                            ShowTextUI.instance.ShowMainText("You approach the box …", () => {                                
                                ShowTextUI.instance.ShowMainText("… a faint sound of breathing can be heard coming from within.", () => {
                                    ShowTextUI.instance.ShowMainText("As you reach for the handle, the breathing becomes faster and faster.", () => {
                                        ShowTextUI.instance.ShowMainText("This doesn't feel right. Maybe some things are better left unopened.");
                                        hydrant3 = true;
                                    });
                                });
                            });
                        },
                        () => { ShowTextUI.instance.ShowMainText("You prefer not to."); });
                });
                break;
            case "door-bathroom":
                ShowTextUI.instance.ShowMainText("A pale wooden door stands in front of you.", () => {
                    ShowTextUI.instance.ShowChoiceText("Enter?",
                        () => {
                            SceneChangeManager.instance.ChangeScene("Bathroom");
                        },
                        () => { ShowTextUI.instance.ShowMainText("Not today, door."); });
                });
                break;
            case "hall-window":
                break;
            case "lights":
                ShowTextUI.instance.ShowMainText("A trusty companion of any hallway. Seeing one makes you feel warm, even though it's all just an illusion.");
                break;
            case "fuse2_on":
                ShowTextUI.instance.ShowMainText("A fuse box that controls the lights in the area. ", () => {
                    ShowTextUI.instance.ShowMainText("The electricity flows joyfully through its copper veins.", () => {
                        ShowTextUI.instance.ShowChoiceText("Take out a fuse?",
                        () => {
                            if (IsWordSelected(spellingWords.bright) && !IsWordSelected(spellingWords.windows))
                            {
                                ShowTextUI.instance.ShowMainText("With a swift motion, take the fuse from the box.", () => {
                                    SyncManager.instance.InvokeTurnOff("light2");
                                    InventoryManager.instance.AddItemToInventory(inventoryItems.fuse);
                                    SyncManager.instance.InvokeTurnOn("fuse2_off");
                                    SyncManager.instance.InvokeTurnOff("fuse2_on");
                                });
                            }
                            else
                            {
                                ShowTextUI.instance.ShowMainText("You’d rather not be in the dark.");
                            }
                        },
                        () => { ShowTextUI.instance.ShowMainText("You decide it would be rude to just take the fuse from the box."); });
                    });
                });

                break;
            case "fuse2_off":
                if(IsItemSelected(inventoryItems.fuse))
                {
                    ShowTextUI.instance.ShowMainText("The fuse clicks in place as the control light turns green. Pleasant.");
                    InventoryManager.instance.RemoveItemFromInventory(inventoryItems.fuse);
                    SyncManager.instance.InvokeTurnOn("light2");
                }
                else
                {
                    ShowTextUI.instance.ShowMainText("A fuse box that controls the lights in the area.", () => {
                        ShowTextUI.instance.ShowMainText("It's missing something. You feel sympathy for the poor thing.");
                    });
                }
                break;
            case "fuse1_on":
                ShowTextUI.instance.ShowMainText("A fuse box that controls the lights in the area.");
                break;
            case "fuse1_off":
                if (IsItemSelected(inventoryItems.fuse))
                {
                    ShowTextUI.instance.ShowMainText("The fuse clicks in place as the control light turns green. Pleasant.");
                    InventoryManager.instance.RemoveItemFromInventory(inventoryItems.fuse);
                    SyncManager.instance.InvokeTurnOn("light1");
                    SyncManager.instance.InvokeTurnOff("ScareArea");
                    SyncManager.instance.InvokeTurnOff("fuse1_off");
                    SyncManager.instance.InvokeTurnOn("fuse1_on");
                }
                else
                {
                    ShowTextUI.instance.ShowMainText("A fuse box that controls the lights in the area.", () => {
                        ShowTextUI.instance.ShowMainText("It's missing something. You feel sympathy for the poor thing.");
                    });
                }
                break;
            case "door-double-glass":
                ShowTextUI.instance.ShowMainText("The tall double doors stand in front of you. The air feels heavy in its presence.", () => {
                ShowTextUI.instance.ShowChoiceText("Are you sure you want to continue?",
                        () => { ShowTextUI.instance.ShowMainText("The tall double doors stand in front of you. The air feels heavy in its presence."); StartWindows.instance.ShowWin(); },
                        () => { ShowTextUI.instance.ShowMainText("You decide it might be a bad idea, for now."); }
                    );
                });
                break;
            case "ScareArea":
                //ShowTextUI.instance.ShowMainText("You’d rather not be in the dark.");
                break;

        }
    }

    public override void WordChanged(spellingWords word, bool important = true)
    {
        base.WordChanged(word, important);
        switch (word)
        {
            case spellingWords.lonely:
                if (IsWordSelected(spellingWords.toilets) || IsWordSelected(spellingWords.chromic_scarfs))
                {
                    SyncManager.instance.InvokeTurnOff("Window_light");
                    SyncManager.instance.InvokeTurnOn("Window_dark");
                }
                else
                {
                    SyncManager.instance.InvokeTurnOff("Window_light");
                    SyncManager.instance.InvokeTurnOff("Window_dark");
                }
                break;
            case spellingWords.bright:
                if (IsWordSelected(spellingWords.toilets) || IsWordSelected(spellingWords.chromic_scarfs))
                {
                    SyncManager.instance.InvokeTurnOn("Window_light");
                    SyncManager.instance.InvokeTurnOff("Window_dark");
                }
                else
                {
                    SyncManager.instance.InvokeTurnOff("Window_light");
                    SyncManager.instance.InvokeTurnOff("Window_dark");
                }
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
            case spellingWords.soap_bars:
                break;
            case spellingWords.rotary_telephones:
                break;
            case spellingWords.sofa:
                break;
            case spellingWords.pile_of_rocks:
                break;
            case spellingWords.toilet:
                break;
            case spellingWords.plumbing:
                SyncManager.instance.InvokeTurnOn("plumbing");
                SyncManager.instance.InvokeTurnOff("hydrant-1");
                SyncManager.instance.InvokeTurnOff("hydrant-2");
                SyncManager.instance.InvokeTurnOff("hydrant-3");
                break;
            case spellingWords.wall_hydrants:
                SyncManager.instance.InvokeTurnOff("plumbing");
                SyncManager.instance.InvokeTurnOn("hydrant-1");
                SyncManager.instance.InvokeTurnOn("hydrant-2");
                SyncManager.instance.InvokeTurnOn("hydrant-3");
                break;
            case spellingWords.windows:
                SyncManager.instance.InvokeTurnOff("Window_light");
                SyncManager.instance.InvokeTurnOff("Window_dark");
                break;
            case spellingWords.toilets:
                if (IsWordSelected(spellingWords.bright))
                {
                    SyncManager.instance.InvokeTurnOn("Window_light");
                    SyncManager.instance.InvokeTurnOff("Window_dark");
                }
                else
                {
                    SyncManager.instance.InvokeTurnOff("Window_light");
                    SyncManager.instance.InvokeTurnOn("Window_dark");
                }
                break;
            case spellingWords.chromic_scarfs:
                if (IsWordSelected(spellingWords.bright))
                {
                    SyncManager.instance.InvokeTurnOn("Window_light");
                    SyncManager.instance.InvokeTurnOff("Window_dark");
                }
                else
                {
                    SyncManager.instance.InvokeTurnOff("Window_light");
                    SyncManager.instance.InvokeTurnOn("Window_dark");
                }
                break;
            case spellingWords.warm:
                break;
            case spellingWords.cold:
                break;
            default:
                break;
        }
    }
}
