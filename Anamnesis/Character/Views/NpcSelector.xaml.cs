﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace Anamnesis.Character.Views
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows.Controls;
	using Anamnesis;
	using Anamnesis.GameData;
	using Anamnesis.Services;
	using Anamnesis.Styles.Drawers;
	using PropertyChanged;

	/// <summary>
	/// Interaction logic for EquipmentSelector.xaml.
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public partial class NpcSelector : UserControl, SelectorDrawer.ISelectorView, INotifyPropertyChanged
	{
		public NpcSelector()
		{
			this.InitializeComponent();
			this.DataContext = this;

			this.Selector.AddItems(GameDataService.ResidentNPCs);
			this.Selector.AddItems(GameDataService.Monsters);

			this.Selector.FilterItems();

			this.PropertyChanged += this.OnSelfPropertyChanged;
		}

		public event DrawerEvent? Close;
		public event DrawerEvent? SelectionChanged;
		public event PropertyChangedEventHandler? PropertyChanged;

		public bool IncludeNpc { get; set; } = true;
		public bool IncludeCharacter { get; set; } = true;
		public bool IncludeMount { get; set; } = true;
		public bool IncludeMinion { get; set; } = true;
		public bool IncludeEffect { get; set; } = true;
		public bool IncludeMonster { get; set; } = true;
		public bool IncludeUnknown { get; set; } = true;

		public bool? IncludeModded { get; set; } = null;

		public INpcResident? Value
		{
			get
			{
				return (INpcResident?)this.Selector.Value;
			}

			set
			{
				this.Selector.Value = value;
			}
		}

		SelectorDrawer SelectorDrawer.ISelectorView.Selector
		{
			get
			{
				return this.Selector;
			}
		}

		private void OnClose()
		{
			this.Close?.Invoke();
		}

		private void OnSelectionChanged()
		{
			this.SelectionChanged?.Invoke();
		}

		private bool OnFilter(object obj, string[]? search = null)
		{
			if (obj is INpcResident npc)
			{
				if (npc.Appearance == null || npc.Appearance.Race == null || npc.Appearance.Race.Key == 0)
					return false;

				if (!this.IncludeNpc && !(obj is Monster))
					return false;

				if (obj is Monster mon)
				{
					if (!this.IncludeCharacter && mon.Type == Monster.Types.Character)
						return false;

					if (!this.IncludeEffect && mon.Type == Monster.Types.Effect)
						return false;

					if (!this.IncludeMinion && mon.Type == Monster.Types.Minion)
						return false;

					if (!this.IncludeMonster && mon.Type == Monster.Types.Monster)
						return false;

					if (!this.IncludeMount && mon.Type == Monster.Types.Mount)
						return false;

					if (!this.IncludeUnknown && mon.Type == Monster.Types.Unknown)
						return false;
				}

				if (this.IncludeModded == true && npc.Mod == null)
					return false;

				if (this.IncludeModded == false && npc.Mod != null)
					return false;

				bool matches = false;
				matches |= SearchUtility.Matches(npc.Singular, search);
				matches |= SearchUtility.Matches(npc.Plural, search);
				matches |= SearchUtility.Matches(npc.Title, search);
				matches |= SearchUtility.Matches(npc.Key.ToString(), search);

				if (npc.Mod != null && npc.Mod.ModPack != null)
				{
					matches |= SearchUtility.Matches(npc.Mod.ModPack.Name, search);
				}

				return matches;
			}

			return false;
		}

		private void OnSelfPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			this.Selector.FilterItems();
		}
	}
}
