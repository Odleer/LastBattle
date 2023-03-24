﻿using System.Linq;
using Content.Client.Computer;
using Content.Client.UserInterface.Controls;
using Content.Shared.Shuttles.BUIStates;
using Content.Shared.Theta.ShipEvent.Console;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Map;

namespace Content.Client.Theta.ShipEvent.Console;

[GenerateTypedNameReferences]
public sealed partial class CannonConsoleWindow : FancyWindow,
    IComputerWindow<CannonConsoleBoundInterfaceState>
{
    private List<CannonInformationInterfaceState> _controlledCannons = new();

    private Dictionary<EntityUid, CannonAmmoStatus> _ammoStatuses = new();

    public CannonConsoleWindow()
    {
        RobustXamlLoader.Load(this);
    }

    public void UpdateState(CannonConsoleBoundInterfaceState scc)
    {
        RadarScreen.UpdateState(scc);
        _controlledCannons = scc.Cannons.Where(i => i.IsControlling).ToList();
        UpdateAmmo();
    }

    public void SetMatrix(EntityCoordinates? coordinates, Angle? angle)
    {
        RadarScreen.SetMatrix(coordinates, angle);
    }

    private void UpdateAmmo()
    {
        foreach (var cannonInformation in _controlledCannons)
        {
            if (!_ammoStatuses.TryGetValue(cannonInformation.Uid, out var status))
            {
                status = new CannonAmmoStatus();
                _ammoStatuses[cannonInformation.Uid] = status;
                AmmoStatusContents.AddChild(status);
            }

            var hasMagazine = cannonInformation is { Ammo: 0, Capacity: 0 };
            status.Update(!hasMagazine, cannonInformation.Ammo, cannonInformation.Capacity);
        }
    }
}
