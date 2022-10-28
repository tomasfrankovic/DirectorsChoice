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
                                SceneChangeManager.instance.ChangeScene("Scene1");
                            });
                        },
                        () => { ShowTextUI.instance.ShowMainText("You feel an unexplainable urge to stay for a bit longer."); });
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
            case "door_bathroom":
                break;
            case "hall-window":
                break;
            case "lights":
                ShowTextUI.instance.ShowMainText("A trusty companion of any hallway. Seeing one makes you feel warm, even though it's all just an illusion.");
                break;
            case "idk8":
                break;
            case "idk9":
                break;
            case "idk10":
                break;
            case "idk11":
                break;
            case "idk12":
                break;
            case "idk13":
                break;

        }
    }

    public override void WordChanged(spellingWords word, bool important = true)
    {
        base.WordChanged(word, important);
        switch (word)
        {
            case spellingWords.lonely:
                break;
            case spellingWords.bright:
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
                break;
            case spellingWords.wall_hydrants:
                break;
            case spellingWords.windows:
                break;
            case spellingWords.toilets:
                break;
            case spellingWords.chromic_scarfs:
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
