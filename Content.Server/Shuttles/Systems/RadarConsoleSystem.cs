using Content.Server.Theta.RadarRenderable;
using Content.Server.UserInterface;
using Content.Shared.Shuttles.BUIStates;
using Content.Shared.Shuttles.Components;
using Content.Shared.Shuttles.Systems;
using Content.Shared.PowerCell;
using Content.Shared.Movement.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
<<<<<<< HEAD
using Content.Shared.PowerCell;
using Content.Shared.Movement.Components;
=======
using System.Numerics;
>>>>>>> r1remote/master

namespace Content.Server.Shuttles.Systems;

public sealed class RadarConsoleSystem : SharedRadarConsoleSystem
{
    [Dependency] private readonly ShuttleConsoleSystem _console = default!;
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly RadarRenderableSystem _radarRenderable = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RadarConsoleComponent, ComponentStartup>(OnRadarStartup);
        SubscribeLocalEvent<RadarConsoleComponent, BoundUIOpenedEvent>(OnUIOpened); // Frontier
    }

    private void OnRadarStartup(EntityUid uid, RadarConsoleComponent component, ComponentStartup args)
    {
        UpdateState(uid, component);
    }

<<<<<<< HEAD
    // Frontier
    private void OnUIOpened(EntityUid uid, RadarConsoleComponent component, ref BoundUIOpenedEvent args)
    {
        UpdateState(uid, component);
    }
    // End Frontier
=======
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<RadarConsoleComponent>();
        while (query.MoveNext(out var uid, out var radar))
        {
            if (!_uiSystem.IsUiOpen(uid, RadarConsoleUiKey.Key))
                continue;
            UpdateState(uid, radar);
        }
    }
>>>>>>> r1remote/master

    protected override void UpdateState(EntityUid uid, RadarConsoleComponent component)
    {
        var xform = Transform(uid);
        var onGrid = xform.ParentUid == xform.GridUid;
        Angle? angle = onGrid ? xform.LocalRotation : Angle.Zero;
        // find correct grid
        while (!onGrid && !xform.ParentUid.IsValid())
        {
            xform = Transform(xform.ParentUid);
            angle = Angle.Zero;
            onGrid = xform.ParentUid == xform.GridUid;
        }

        EntityCoordinates? coordinates = onGrid ? xform.Coordinates : null;
<<<<<<< HEAD
        Angle? angle = onGrid ? xform.LocalRotation : null;
=======

>>>>>>> r1remote/master
        if (component.FollowEntity)
        {
            coordinates = new EntityCoordinates(uid, Vector2.Zero);
            angle = Angle.FromDegrees(180); // Frontier: Angle.Zero<Angle.FromDegrees(180)
        }

        var radarState = new RadarConsoleBoundInterfaceState(
            new NavInterfaceState(component.MaxRange, GetNetCoordinates(coordinates), angle, new Dictionary<NetEntity, List<DockingPortState>>()),
            new DockingInterfaceState(),
            _radarRenderable.GetObjectsAround(uid, component)
        );

        _uiSystem.SetUiState(uid, RadarConsoleUiKey.Key, radarState);
    }

<<<<<<< HEAD
            state.RotateWithEntity = !component.FollowEntity;

            // Frontier: ghost radar restrictions
            if (component.MaxIffRange != null)
                state.MaxIffRange = component.MaxIffRange.Value;
            state.HideCoords = component.HideCoords;
            // End Frontier

            _uiSystem.SetUiState(uid, RadarConsoleUiKey.Key, new NavBoundUserInterfaceState(state));
        }
=======
    public bool HasFlag(RadarConsoleComponent radar, RadarRenderableGroup e)
    {
        return radar.TrackedGroups.HasFlag(e);
>>>>>>> r1remote/master
    }
}
