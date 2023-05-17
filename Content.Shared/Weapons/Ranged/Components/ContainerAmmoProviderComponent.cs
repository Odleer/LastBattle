﻿using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Ranged.Components;

/// <summary>
///     Handles pulling entities from the given container to use as ammunition.
/// </summary>
[RegisterComponent]
public sealed class ContainerAmmoProviderComponent : AmmoProviderComponent
{
    [DataField("container", required: true)]
    [ViewVariables]
    public string Container = "storagebase";

    [DataField("provider")]
    [ViewVariables]
    public EntityUid? ProviderUid;
}
