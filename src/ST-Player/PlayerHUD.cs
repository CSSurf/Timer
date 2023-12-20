namespace SurfTimer;

internal class PlayerHUD
{
    private Player _player;

    public PlayerHUD(Player Player)
    {
        _player = Player;
    }

    private string FormatHUDElementHTML(string title, string body, string color, string size = "m")
    {
        if (title != "")
        {
            if (size == "m")
                return $"{title}: <font color='{color}'>{body}</font>";
            else
                return $"<font class='fontSize-{size.ToLower()}'>{title}: <font color='{color}'>{body}</font></font>";
        }

        else
        {
            if (size == "m")
                return $"<font color='{color}'>{body}</font>";
            else
                return $"<font class='fontSize-{size.ToLower()}' color='{color}'>{body}</font>";
        }
    }

    public string FormatTime(int ticks) // https://github.com/DEAFPS/SharpTimer/blob/e4ef24fff29a33c36722d23961355742d507441f/Utils.cs#L38
    {
        TimeSpan time = TimeSpan.FromSeconds(ticks / 64.0);
        int millis = (int)(ticks % 64 * (1000.0 / 64.0));
        return $"{time.Minutes:D2}:{time.Seconds:D2}.{millis:D3}";
    }

    public void Display()
    {
        if (_player.Controller.IsValid && _player.Controller.PawnIsAlive)
        {
            // Timer Module
            string timerColor = "#79d1ed";
            if (_player.Timer.IsRunning)
            {
                if (_player.Timer.PracticeMode)
                    timerColor = "#F2C94C";
                else
                    timerColor = "#2E9F65";
            }
            string timerModule = FormatHUDElementHTML("", FormatTime(_player.Timer.Ticks), timerColor);

            // Velocity Module
            float sqVelocity = _player.Controller.PlayerPawn.Value!.AbsVelocity.X * _player.Controller.PlayerPawn.Value!.AbsVelocity.X
                               + _player.Controller.PlayerPawn.Value!.AbsVelocity.Y * _player.Controller.PlayerPawn.Value!.AbsVelocity.Y;

            if (SurfTimer.pluginCfg.Config.IncludeZVelocity)
                sqVelocity += _player.Controller.PlayerPawn.Value!.AbsVelocity.Z * _player.Controller.PlayerPawn.Value!.AbsVelocity.Z;

            float velocity = (float) Math.Sqrt(sqVelocity);
            string velocityModule = FormatHUDElementHTML("Speed", velocity.ToString("000"), "#79d1ed") + " u/s";
            // Rank Module
            string rankModule = FormatHUDElementHTML("Rank", "N/A", "#7882dd"); // IMPLEMENT IN PlayerStats
            // PB & WR Modules
            string pbModule = FormatHUDElementHTML("PB", _player.Stats.PB[0,0] > 0 ? FormatTime(_player.Stats.PB[0,0]) : "N/A", "#7882dd"); // IMPLEMENT IN PlayerStats
            string wrModule = FormatHUDElementHTML("WR", "N/A", "#7882dd"); // IMPLEMENT IN PlayerStats

            // Build HUD
            string hud = $"{timerModule}<br>{velocityModule}<br>{pbModule} | {rankModule}<br>{wrModule}";

            // Display HUD
            _player.Controller.PrintToCenterHtml(hud);
        }
    }
}
