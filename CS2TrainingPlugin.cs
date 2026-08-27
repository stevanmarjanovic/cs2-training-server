using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CS2TrainingPlugin.Maps;

namespace CS2TrainingPlugin;

public class CS2TrainingPlugin : BasePlugin
{
    public override string ModuleName => "CS2 Training";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Stevan \"sttiw\" Marjanovic";
    
    private const string FrameModelName = "models/props/de_dust/hr_dust/dust_fences/dust_chainlink_fence_001_128.vmdl";
    private const string LinksModelName = "models/props/de_dust/hr_dust/dust_fences/dust_chainlink_fence_001_128_links.vmdl";
    private const float FenceLength = 128f;
    
    private int _roundAttackIndex = 0;

    public override void Load(bool hotReload)
    {
        Console.WriteLine("CS2 Training plugin loaded");
        RegisterListener<Listeners.OnServerPrecacheResources>(manifest =>
        {
            manifest.AddResource(FrameModelName);
            manifest.AddResource(LinksModelName);
        });
    }

    public override void Unload(bool hotReload)
    {
        Console.WriteLine("CS2 Training plugin unloaded");
    }

    [GameEventHandler]
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        foreach (var wall in Dust2Walls.RoundAttacks[_roundAttackIndex])
        {
            var position = new Vector(wall.X, wall.Y, wall.Z);
            CreateChainFence(position, wall.Yaw, wall.Amount);
        }
        
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        _roundAttackIndex++;
        if (_roundAttackIndex == Dust2Walls.RoundAttacks.Length)
        {
            _roundAttackIndex = 0;
        }

        return HookResult.Continue;
    }
    
    /// <summary>
    /// Handles the "css_pos" console command, printing the player's current position and rotation.
    /// </summary>
    /// <param name="player">The player controller associated with the command or null if unavailable.</param>
    /// <param name="command">The command information and context for replying back with results.</param>
    [ConsoleCommand("css_pos", "Print your current position and rotation")]
    public void OnPositionCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid) return;
        
        var pawn = player.PlayerPawn.Value;

        if (pawn == null || !pawn.IsValid) return;

        var position = pawn.AbsOrigin;
        var rotation = pawn.EyeAngles;

        if (position == null) return;
        
        command.ReplyToCommand(
            $"Position: X={position.X:F2}, Y={position.Y:F2}, Z={position.Z:F2}"
        );
        
        command.ReplyToCommand(
            $"Rotation: Pitch={rotation.X:F2}, Yaw={rotation.Y:F2}, Roll={rotation.Z:F2}"
        );
    }

    [ConsoleCommand("css_attack", "Spawn walls for a random attack")]
    public void OnTestPropCommand(CCSPlayerController? player, CommandInfo command)
    {
        var RandomAttack = Dust2Walls.RoundAttacks[
            Random.Shared.Next(Dust2Walls.RoundAttacks.Length)
        ];
        
        foreach (var wall in RandomAttack)
        {
            var position = new Vector(wall.X, wall.Y, wall.Z);
            CreateChainFence(position, wall.Yaw, wall.Amount);
        }
        
        command.ReplyToCommand("Test prop spawned successfully.");
    }

    private void CreateChainFence(
        Vector startingPosition,
        float yaw,
        int amount
    )
    {
        var radians = (yaw + 90) *  MathF.PI / 180;
        
        var stepX = MathF.Cos(radians) * FenceLength;
        var stepY = MathF.Sin(radians) * FenceLength;

        for (var i = 0; i < amount; i++)
        {
            var position = new Vector(
                startingPosition.X + stepX * i,
                startingPosition.Y + stepY * i,
                startingPosition.Z
            );
            
            var rotation = new QAngle(0, yaw, 0);
            
            CreateFenceSegment(position, rotation);
        }
    }

    private void CreateFenceSegment(Vector position, QAngle rotation)
    {
        var frame = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
        var links = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");

        if (frame == null || !frame.IsValid ||
            links == null || !links.IsValid)
        {
            return;
        }

        frame.SetModel(FrameModelName);
        links.SetModel(LinksModelName);
        
        links.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_PROPS;
        links.Collision.SolidType = SolidType_t.SOLID_VPHYSICS;

        var immovable = new Vector(0, 0, 0);

        frame.Teleport(position, rotation, immovable);
        links.Teleport(position, rotation, immovable);

        frame.DispatchSpawn();
        links.DispatchSpawn();
    }
}