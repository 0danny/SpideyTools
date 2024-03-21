using CommunityToolkit.Mvvm.ComponentModel;
using SpideyTools.Core.Helper;
using SpideyTools.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpideyTools.Core.Mods
{
    public class CharacterSwap
    {
        private readonly byte[] originalBytes = { 0x8B, 0x04, 0x8D, 0xC4, 0xFF, 0x8C, 0x00 };

        //Characters in the game.
        public ObservableCollection<CharacterMod> characterMods = new()
        {
            new CharacterMod { Name = "Original", Address = 0x0, InternalName = "original" },
            new CharacterMod { Name = "Peter Student", Address = 0x008C618C, InternalName = "peterstudent" },
            new CharacterMod { Name = "Ross Spidey", Address = 0x008C617C, InternalName = "rossspidey" },
            new CharacterMod { Name = "Spider-Man", Address = 0x008D0398, InternalName = "spiderman" },
            new CharacterMod { Name = "Spidey Wrestle", Address = 0x008C619C, InternalName = "spideywrestle" },
            new CharacterMod { Name = "Btg Spidey", Address = 0x008C6138, InternalName = "btgspidey" },
            new CharacterMod { Name = "Cop Spidey", Address = 0x008C6110, InternalName = "copspidey" },
            new CharacterMod { Name = "Freak Spidey", Address = 0x008C611C, InternalName = "freakspidey" },
            new CharacterMod { Name = "Heli Spidey", Address = 0x008C6100, InternalName = "helispidey" },
            new CharacterMod { Name = "Mj Spidey", Address = 0x008C6170, InternalName = "mjspidey" },
            new CharacterMod { Name = "Sci Spidey", Address = 0x008C6154, InternalName = "scispidey" },
            new CharacterMod { Name = "Shock Spidey", Address = 0x008C6160, InternalName = "shockspidey" },
            new CharacterMod { Name = "Sth Spidey", Address = 0x008C612C, InternalName = "sthspidey" },
            new CharacterMod { Name = "Thug Spidey", Address = 0x008C6144, InternalName = "thugspidey" },
            new CharacterMod { Name = "Goblin", Address = 0x008C60F4, InternalName = "plrgoblin" },
            new CharacterMod { Name = "Ross Goblin", Address = 0x008C60E4, InternalName = "plrrossgoblin" },
        };

        public void swap(string characterName)
        {
            switch (characterName)
            {
                case "plrrossgoblin":
                    //OPEN THE GATES FOR ALEX ROSS GREEN GOBLIN.
                    revertSwap();
                    
                    Memory.patchInt(0x008CFDD4, 0);
                    break;
                case "original":
                    revertSwap();
                    break;
                default:
                    //Turn off alex ross goblin.
                    Memory.patchInt(0x008CFDD4, 0x1B00DEAD);

                    CharacterMod? charMod = characterMods.Where(elem => elem.InternalName == characterName).FirstOrDefault();

                    if (charMod != null)
                    {
                        Logger.Log($"Patching with address: {charMod.Address}");

                        //Swap the character into v24.
                        patchCharacterSwap(0x5BE342, charMod.Address);
                    }
                    else
                    {
                        Logger.Log($"Character {characterName} does not exist.");
                    }

                    break;
            }
        }

        public void revertSwap()
        {
            //Put original bytes back.
            Memory.patchInstruction(0x5BE342, originalBytes);

            //Turn off Alex Ross Green Goblin.
            Memory.patchInt(0x008CFDD4, 0x1B00DEAD);
        }

        private void patchCharacterSwap(int address, int value)
        {
            byte[] instruction = new byte[7];
            instruction[0] = 0xB8; // opcode for mov eax, imm32
            BitConverter.GetBytes(value).CopyTo(instruction, 1); // Copy the 32-bit value into the array starting at index 1

            //NOP Padding
            //mov with indirect addressing is 2 bytes bigger than direct addressing
            instruction[5] = 0x90; // NOP
            instruction[6] = 0x90; // NOP

            Memory.patchInstruction(address, instruction);
        }
    }
}
