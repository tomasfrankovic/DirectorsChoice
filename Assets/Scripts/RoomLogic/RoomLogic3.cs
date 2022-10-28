using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic3 : AbstractRoomLogic
{
    bool cabinetLocked = true;
    bool keyTook;
    bool flashlightTook;

    public override void Init()
    {
        for (int i = 0; i < SpellingWordsManager.instance.selectedWordsList.Count; i++)
            WordChanged(SpellingWordsManager.instance.selectedWordsList[i]);
    }

    public override void InteractionHappened(string interaction, bool important = true)
    {
        base.InteractionHappened(interaction, important);

        switch (interaction)
        {
            case "radiator":
                ShowTextUI.instance.ShowMainText("A trusty companion of any hallway. Seeing one makes you feel warm, even though it's all just an illusion.");
                break;
            case "sink":
                if(cabinetLocked)
                {
                    if(IsItemSelected(inventoryItems.hall_key))
                    {
                        ShowTextUI.instance.ShowMainText("The key fits snugly in the keyhole.", () => {
                            ShowTextUI.instance.ShowMainText("With a twist and a satisfying crackle, the cabinet unlocks, revealing its insides.", () =>
                            {
                                ShowTextUI.instance.ShowMainText("You find a small battery cell.");
                                InventoryManager.instance.AddItemToInventory(inventoryItems.battery);
                                cabinetLocked = false;
                            });
                        });
                    }
                    else
                    {
                        ShowTextUI.instance.ShowMainText("A firm cabinet stands in front of you. Opening its doors, you challenge its imposing nature. However, the cabinet is victorious and its doors remain locked.");
                    }
                }
                else
                {
                    ShowTextUI.instance.ShowMainText("You dart over the content of the cabinet, however, you don’t find anything interesting.");
                }
                break;
            case "light":
                ShowTextUI.instance.ShowMainText("A tiny little light compared to its hallway brethren.", () => {
                    ShowTextUI.instance.ShowMainText("Regardless of that, the light is determined to shine just as brightly.");
                });
                break;
            case "bathtub":
                if(!flashlightTook)
                {
                    ShowTextUI.instance.ShowMainText("A tiny little light compared to its hallway brethren.", () => {
                        ShowTextUI.instance.ShowMainText("A tiny little light compared to its hallway brethren.", () => {
                            ShowTextUI.instance.ShowMainText("Regardless of that, the light is determined to shine just as brightly.");
                            InventoryManager.instance.AddItemToInventory(inventoryItems.flashlight);
                            flashlightTook = true;
                        });
                    });
                }
                else if(!keyTook && IsWordSelected(spellingWords.soap_bars))
                {
                    ShowTextUI.instance.ShowMainText("A regular, run-of-the-mill bathtub, nothing special here, aside from the strangely scented soap.", () => {
                        ShowTextUI.instance.ShowMainText("Shortly before leaving, you notice that the soap has a strange color. Looking closer you notice an outline of a key.", () =>
                        {
                            ShowTextUI.instance.ShowChoiceText("Would you like to try to free it?",
                                () => {
                                    ShowTextUI.instance.ShowMainText("The soap easily crumbles in your soft paws, leaving nothing but the hard metal key.");
                                    keyTook = true;
                                    InventoryManager.instance.AddItemToInventory(inventoryItems.hall_key);
                                },
                                () => { ShowTextUI.instance.ShowMainText("You prefer not to.");
                            });
                        });
                    });
                }
                else
                {
                    ShowTextUI.instance.ShowMainText("A regular, run-of-the-mill bathtub, nothing special here, aside from the strangely scented soap.");
                }
                break;
            case "door-bathroom":
                ShowTextUI.instance.ShowMainText("A pale wooden door stands in front of you.", () => {
                    ShowTextUI.instance.ShowChoiceText("Enter?",
                        () => {
                            SceneChangeManager.instance.ChangeScene("Hallway");
                        },
                        () => { ShowTextUI.instance.ShowMainText("Not today, door."); });
                });
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
