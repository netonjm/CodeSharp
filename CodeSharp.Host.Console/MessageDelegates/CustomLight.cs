using System;
using CodeSharp.Accessories;

namespace CodeSharp.MessageDelegates
{
	class CustomLightMessageDelegate : LightMessageDelegate
	{
		bool actualValue;

		public CustomLightMessageDelegate (LightAccessory lightAccessory)
			: base (lightAccessory)
		{

		}

		protected override void OnChangePower (bool value)
		{
			actualValue = value;
			Console.WriteLine ($"[Net][{Accessory.Name}][Set] " + value);
		}

		protected override bool OnGetPower ()
		{
			return actualValue;
		}

		public override void OnIdentify ()
		{
			Console.WriteLine ($"[Net][{Accessory.Name}] Identified.");
		}
	}
}
