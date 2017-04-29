using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;
using RoutingEffects = PresetPedalForms;
using PlatformEffects = PresetPedalForms.Effects.iOS;

[assembly: ExportEffect(typeof(PlatformEffects.EntryCapitalizeKeyboard), nameof(RoutingEffects.EntryCapitalizeKeyboard))]
namespace PresetPedalForms.Effects.iOS
{
	[Preserve(AllMembers = true)]
	public class EntryCapitalizeKeyboard : PlatformEffect
	{
		private UITextAutocapitalizationType _old;

		protected override void OnAttached()
		{
			var editText = Control as UITextField;
			if(editText == null)
				return;

			_old = editText.AutocapitalizationType;
			editText.AutocapitalizationType = UITextAutocapitalizationType.Words;
		}

		protected override void OnDetached()
		{
			var editText = Control as UITextField;
			if(editText == null)
				return;

			editText.AutocapitalizationType = _old;
		}
	}
}