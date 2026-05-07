# Déploiement BWS Bible API sur Podman

## 📋 Prérequis

- **Podman** 5.8.2+ installé
- **podman-compose** (optionnel, pour utiliser docker-compose.yml)
- **Python 3.x** (si vous utilisez podman-compose)
- `.NET SDK 8.0` (seulement si vous compilez localement sans Docker)

## 🚀 Démarrage rapide

### 1. **Avec docker-compose (recommandé)**

```bash
# Depuis la racine du projet
podman-compose up -d

# Vérifier le statut
podman-compose ps

# Voir les logs
podman-compose logs -f bws-bible-api
```

### 2. **Avec podman build + podman run**

```bash
# Build l'image
podman build -t bws-bible-api .

# Run le container
podman run -d \
  -p 5000:5000 \
  -p 5001:5001 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -v $(pwd)/Bws.Bible.Infrastructure/Storage:/app/Storage \
  --name bws-bible-api \
  bws-bible-api
```

## 📂 Structure des fichiers

```
Bws.Bible.Api/
├── Dockerfile              # Multi-stage build optimisé
├── docker-compose.yml      # Orchestration locale
├── .dockerignore          # Optimise le build context
└── Bws.Bible.Infrastructure/
    └── Storage/           # Fichiers bible (LSG.bible, etc.)
        ├── LSG.bible      # Bible Louis Segond (français)
        ├── WLC.bible      # Hebrew Old Testament
        ├── SBLGNT.bible   # Greek New Testament
        └── OST.bible      # Jean-Frédéric Osterwald Translation (français)
```

## 🔍 Vérifier que l'API fonctionne

### Health Checks

```bash
# Liveness probe
curl http://localhost:5000/health/live

# Readiness probe (vérifie la présence de LSG.bible)
curl http://localhost:5000/health/ready
```

### Documentation Swagger

```
http://localhost:5000/swagger
```

### Exemples d'API

```bash
# Lister les bibles disponibles
curl http://localhost:5000/explore/bibles

# Chercher un verset
curl http://localhost:5000/search/bibles/LSG/verses?word=amour
```

## 🛑 Arrêter l'API

```bash
# Avec docker-compose
podman-compose down

# Avec podman run
podman stop bws-bible-api
podman rm bws-bible-api
```

## 📝 Configuration

Les fichiers d'application sont dans `Bws.Bible.Api/`:
- `appsettings.json` - Configuration globale
- `appsettings.Development.json` - Configuration dev
- `appsettings.Production.json` - Configuration prod

Le container utilise `ASPNETCORE_ENVIRONMENT=Development` pour le déploiement local.

## 🔐 Sécurité

- L'application s'exécute sous l'utilisateur non-root `appuser`
- Les fichiers `.bible` sont montés via volume pour une gestion flexible
- Health checks automatiques activés

## 📊 Ports

- **5000** : HTTP (développement)
- **5001** : HTTPS (optionnel)

## 🐛 Dépannage

### Le container ne démarre pas

```bash
# Vérifier les logs
podman logs bws-bible-api

# Vérifier que Podman fonctionne
podman ps -a
```

### Fichiers .bible manquants

Assurez-vous que les fichiers existent dans:
```
Bws.Bible.Infrastructure/Storage/
```

Si les fichiers sont manquants, le health check readiness échouera.

### Port déjà en utilisation

```bash
# Changer le port dans docker-compose.yml ou podman run
# Exemple: "5010:5000" mappe le port 5010 du host au 5000 du container
```