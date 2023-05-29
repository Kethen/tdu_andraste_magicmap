using Andraste.Payload.ModManagement;
using Andraste.Payload.Native;
using System;
using NLog;

namespace KatieCookie.tdu
{
	public class MagicMap : BasePlugin
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private bool _enabled;
		public override bool Enabled
		{
			get {
				return _enabled;
			}
			set
			{
				_enabled = value;
				IntPtr location = new IntPtr(0x009a6b63);
				byte [] expectation = new byte[]{
					// MOV ECX,dword ptr [ESP + 0x14], aka set param2 to param_1_00
					0x8b, 0x4c, 0x24, 0x14,
					// MOV EAX,EBI
					0x8b, 0xc3,
					// SUB EAX, EDI
					0x2b, 0xc7,
					// CDQ
					0x99,
					// SUB EAX, EDX
					0x2b, 0xc2,
					// MOV ESI, EAX,
					0x8b, 0xf0,
					// SAR ESI, 0x1
					0xd1, 0xfe,
					// ADD ESI, EDI
					0x03, 0xf7,
					// LEA EDX, [ESI + ESI * 0x2]
					0x8d, 0x14, 0x76,
					// LEA EDX,[ECX + EDX*0x8]
					0x8d, 0x14, 0xd1,
					// MOV EAX, EBP
					0x8b, 0xc5,
					// CALL 0x009a6800, this function takes name hash, finds the matching entry in bnk1.map, then populate the buffer at EBP(EAX) with the content
					0xe8, 0x7f, 0xfc, 0xff, 0xff,
					// MOV EAX, dword ptr [EBP]
					0x8b, 0x45, 0x00,
					// MOV ECX,dword ptr [ESP + 0x18]
					0x8b, 0x45, 0x24, 0x18,
					// CMP ECX, EAX
					0x3b, 0xc8,
					// JZ 0x009a6ba3, return EBP
					0x74, 0x17
				};
				byte [] to_apply = new byte []{
					// nop * 4
					0x90, 0x90, 0x90, 0x90,
					// MOV eax, dword ptr [ESP + 0x18], aka set EAX to the content of param_3, the name hash
					0x8b, 0x44, 0x24, 0x18,
					// MOV dword ptr [EBP], EAX, set the hash to the buffer directly
					0x89, 0x45, 0x00,
					// ADD EBP, 0x4, point EBP to first size
					0x83, 0xc5, 0x04,
					// MOV dword ptr [EBP], 0x0, write first size to 0 like magicmap
					0xc7, 0x45, 0x00, 0x00, 0x00, 0x00, 0x00,
					// ADD EBP, 0x8, point EBP to second size
					0x83, 0xc5, 0x08,
					// MOV dword ptr [EBP], 0x0, write second size to 0 like magicmap
					0xc7, 0x45, 0x00, 0x00, 0x00, 0x00, 0x00,
					// SUB EBP, 0xc, point EBP back to the hash
					0x83, 0xed, 0x0c,
					// MOV ECX, EAX, just write param_1_00 directly, it seems to be an out param of sort
					0x8b, 0xc8,
					// nop * 3
					0x90, 0x90, 0x90,
					// JMP 0x009a6ba3, return EBP
					0xeb, 0x17
				};

				InstructionPatcher patcher = new InstructionPatcher(location, expectation, to_apply);
				if (value){
					patcher.Patch(true);
					Logger.Info("Andraste magic map is now enabled");
				}else{
					patcher.Patch(false);
					Logger.Info("Andraste magic map is now disabled");
				}
			}
		}

		protected override void PluginLoad()
		{
		}

		protected override void PluginUnload()
		{
		}
	}
}