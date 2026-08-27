using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace CS2TrainingPlugin;

public class CS2TrainingPlugin : BasePlugin
{
    public override string ModuleName => "CS2 Training";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Stevan \"sttiw\" Marjanovic";

    public override void Load(bool hotReload)
    {
        Console.WriteLine("CS2 Training plugin loaded");
    }

    public override void Unload(bool hotReload)
    {
        Console.WriteLine("CS2 Training plugin unloaded");
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

    [ConsoleCommand("css_testprop", "Spawn a test prop")]
    public void OnTestPropCommand(CCSPlayerController? player, CommandInfo command)
    {
        var prop = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");

        if (prop == null || !prop.IsValid)
        {
            command.ReplyToCommand("Failed to spawn test prop.");
            return;
        }
        
        prop.SetModel("models/props/de_dust/dust_crates/dust_crate_style_01_32x64x64.vmdl");

        var position = new Vector(-569.97f, 1964.01f, -117.31f);
        var rotation = new QAngle(0, 90, 0);

        prop.Teleport(position, rotation, new Vector(0, 0, 0));
        prop.DispatchSpawn();
        
        command.ReplyToCommand("Test prop spawned successfully.");
    }
}