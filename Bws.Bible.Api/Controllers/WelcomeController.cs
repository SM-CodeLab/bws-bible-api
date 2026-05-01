using Microsoft.AspNetCore.Mvc;

namespace Bws.Bible.Api.Controllers;

/// <summary>
/// Accueil
/// </summary>
[Route("")]
[ApiController]
public class WelcomeController : ControllerBase
{
    /// <summary>
    /// Afficher le message de bienvenue
    /// </summary>
    [HttpGet("/")]
    [HttpGet("/index.html")]
    public ContentResult Index()
    {
        var html = @"
<!DOCTYPE html>
<html lang=""fr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Bible Web Services - API Bible</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
        }
        .container {
            background: white;
            border-radius: 10px;
            padding: 40px;
            margin: 20px auto;
            box-shadow: 0 10px 30px rgba(0,0,0,0.1);
        }
        h1 {
            color: #2c3e50;
            text-align: center;
            margin-bottom: 10px;
            font-size: 2.5em;
        }
        .subtitle {
            text-align: center;
            color: #7f8c8d;
            font-size: 1.2em;
            margin-bottom: 40px;
        }
        .section {
            margin: 30px 0;
            padding: 20px;
            border-left: 4px solid #3498db;
            background: #f8f9fa;
            border-radius: 5px;
        }
        .section h2 {
            color: #2c3e50;
            margin-top: 0;
            font-size: 1.5em;
        }
        .endpoint {
            background: #ecf0f1;
            padding: 10px;
            margin: 10px 0;
            border-radius: 5px;
            font-family: 'Courier New', monospace;
            color: #e74c3c;
        }
        .btn {
            display: inline-block;
            padding: 12px 24px;
            background: #3498db;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin: 10px 10px 10px 0;
            transition: background 0.3s;
        }
        .btn:hover {
            background: #2980b9;
        }
        .btn.secondary {
            background: #2ecc71;
        }
        .btn.secondary:hover {
            background: #27ae60;
        }
        .features {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 20px;
            margin: 30px 0;
        }
        .feature {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            text-align: center;
            border: 1px solid #e9ecef;
        }
        .feature h3 {
            color: #2c3e50;
            margin-top: 0;
        }
        .footer {
            text-align: center;
            margin-top: 40px;
            padding-top: 20px;
            border-top: 1px solid #ecf0f1;
            color: #7f8c8d;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <h1>📜 Bible Web Services - API Bible</h1>
        <p class=""subtitle"">Une API REST moderne pour accéder aux textes bibliques</p>

        <div class=""section"">
            <h2>🚀 Démarrage rapide</h2>
            <p>Commencez par explorer la documentation interactive :</p>
            <a href=""/swagger"" class=""btn"">📖 Documentation Swagger</a>
            <a href=""/health/live"" class=""btn secondary"">❤️ Health Check</a>
        </div>

        <div class=""section"">
            <h2>📚 Fonctionnalités principales</h2>
            <div class=""features"">
                <div class=""feature"">
                    <h3>🔍 Recherche</h3>
                    <p>Recherchez des mots ou expressions dans les textes bibliques</p>
                    <div class=""endpoint"">GET /search/{bible}/{mots}</div>
                </div>
                <div class=""feature"">
                    <h3>🗺️ Exploration</h3>
                    <p>Naviguez dans la structure des livres et chapitres</p>
                    <div class=""endpoint"">GET /explore/{bible}/{livre}</div>
                </div>
                <div class=""feature"">
                    <h3>⚖️ Comparaison</h3>
                    <p>Comparez les mêmes passages entre différentes traductions</p>
                    <div class=""endpoint"">GET /compare/{bible1}/{bible2}</div>
                </div>
            </div>
        </div>

        <div class=""section"">
            <h2>💡 Exemples d'utilisation</h2>
            <p>Voici quelques exemples d'endpoints populaires :</p>
            <div class=""endpoint"">GET /search/LSG/Jésus</div>
            <div class=""endpoint"">GET /explore/LSG/Matt/1</div>
            <div class=""endpoint"">GET /compare/LSG/OST/Matt/1:1</div>
            <div class=""endpoint"">GET /health/ready</div>
        </div>

        <div class=""section"">
            <h2>📋 Bibles disponibles</h2>
            <p>L'API propose plusieurs traductions bibliques :</p>
            <ul>
                <li><strong>LSG</strong> - Louis Segond 1910 (Français)</li>
                <li><strong>OST</strong> - Ostervald 1996 (Français)</li>
                <li><strong>SBLGNT</strong> - Greek New Testament (Grec koïné)</li>
                <li><strong>WLC</strong> - Westminster Leningrad Codex (Hébreu biblique)</li>
            </ul>
        </div>

        <div class=""section"">
            <h2>🛠️ Outils de développement</h2>
            <p>Utilisez notre collection Bruno pour tester l'API :</p>
            <a href=""https://github.com/SM-CodeLab/bws-bible-api/tree/dev/bruno"" class=""btn"">📁 Collection Bruno</a>
            <a href=""https://github.com/SM-CodeLab/bws-bible-api"" class=""btn secondary"">📂 Code source GitHub</a>
        </div>

        <div class=""footer"">
            <p>🚀 Propulsé par Microsoft .NET 8.0 | 📧 <a href=""mailto:contact@samuel-meyer.fr"">contact@samuel-meyer.fr</a></p>
            <p>🔗 <a href=""https://github.com/SM-CodeLab/bws-bible-api/blob/dev/TERMS.md"">Conditions d'utilisation</a> |
               <a href=""https://github.com/SM-CodeLab/bws-bible-api/blob/dev/LICENSE.md"">Licence</a></p>
        </div>
    </div>
</body>
</html>";

        return Content(html, "text/html; charset=utf-8");
    }
}
