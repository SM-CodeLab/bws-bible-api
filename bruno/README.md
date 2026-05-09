
# Tests API via Bruno - Bws.Bible.Api

Collection de tests Bruno pour valider l'API Bible.

## Installation

Télécharger Bruno : https://www.usebruno.com/

## Structure

```
bruno/
├── API Bible (Bws.Bible.Api)/
│   ├── environments/
│   │   └── Dev_FR.yml              # Environnement de développement
│   ├── Health/
│   │   ├── 01. HealthCheck Liveness.yml
│   │   └── 02. HealthCheck Readiness.yml
│   ├── Search/
│   │   ├── 01. Search Bible.yml
│   │   ├── 02. Search Book.yml
│   │   ├── 03. Search Chapter.yml
│   │   ├── 04. Search Verse.yml
│   │   └── 05. Search Verses.yml
│   ├── Explore/
│   │   ├── 01. Explore Bible.yml
│   │   ├── 02. Explore Book.yml
│   │   ├── 03. Explore Chapter.yml
│   │   ├── 04. Explore Verse.yml
│   │   └── 05. Explore Verses.yml
│   └── Compare/
│       ├── 01. Compare Bible.yml
│       ├── 02. Compare Book.yml
│       ├── 03. Compare Chapter.yml
│       ├── 04. Compare Verse.yml
│       └── 05. Compare Verses.yml
└── README.md
```

## Configuration de l'environnement

L'environnement `Dev_FR` contient les variables suivantes :

| Variable | Valeur par défaut | Description |
|----------|-------------------|-------------|
| `apiUrl` | `https://localhost:5001` | URL de base de l'API |
| `idBible` | `LSG` | Identifiant de la bible par défaut |
| `idBook` | `40` | Identifiant du livre (ex. 40 = Matthieu) |
| `idChapter` | `1` | Numéro du chapitre |
| `searchWords` | `Jésus` | Mots à rechercher |
| `idBibleToCompare` | `OST` | Bible secondaire pour les comparaisons |

**Modification des variables :**
1. Dans Bruno, cliquer sur l'onglet `Environments` (en bas à gauche)
2. Sélectionner `Dev_FR`
3. Modifier les variables selon vos besoins
4. Les modifications sont sauvegardées automatiquement

## Lancer les tests

### Prérequis

1. L'API doit être en cours d'exécution :
   ```bash
   cd Bws.Bible.Api
   dotnet run
   ```

2. Bruno doit être ouvert avec la collection chargée

### Procédure

#### Option 1 : Exécuter la collection entière (recommandé)

1. **Sélectionner l'environnement :**
   - En bas à gauche de Bruno, cliquer sur le menu déroulant `Environments`
   - Sélectionner `Dev_FR`

2. **Lancer les tests :**
   - Cliquer sur l'icône de lecture (▶️) à côté du nom de la collection `API Bible (Bws.Bible.Api)`
   - Ou utiliser le raccourci clavier : `Ctrl + Shift + Enter`
   - Bruno exécutera toutes les requêtes séquentiellement

3. **Vérifier les résultats :**
   - Les tests réussis affichent une coche verte (✓)
   - Les tests échoués affichent une croix rouge (✗)
   - Consulter l'onglet `Tests` dans la réponse pour les détails

#### Option 2 : Exécuter une requête individuelle

1. **Sélectionner l'environnement :** `Dev_FR` (même procédure ci-dessus)

2. **Sélectionner une requête :**
   - Dans l'arborescence à gauche, cliquer sur une requête (ex. `01. HealthCheck Liveness`)

3. **Envoyer la requête :**
   - Cliquer sur le bouton `Send` ou `Ctrl + Enter`

4. **Consulter la réponse :**
   - Les résultats des tests s'affichent dans le panneau de droite, onglet `Tests`

### Interprétation des tests

Chaque requête contient 3 assertions critiques :

- ✓ **Status code is 200** : Vérifie que la requête est réussie
- ✓ **Response is JSON** : Vérifie le Content-Type `application/json`
- ✓ **Response has data property** : Vérifie la présence des clés attendues

Exemple de réponse réussie :
```
✓ Status code is 200
✓ Response is JSON
✓ Response has data property
```

## Dépannage

### "apiUrl is undefined"
- Vérifier que l'environnement `Dev_FR` est sélectionné (en bas à gauche)
- S'assurer que l'API est en cours d'exécution sur `https://localhost:5001`

### Erreur 404 ou connection refused
- L'API n'est pas lancée. Exécuter `dotnet run` dans le répertoire `Bws.Bible.Api`
- Vérifier l'URL dans `Dev_FR.yml` (par défaut : `https://localhost:5001`)

### Tests échouent avec des paramètres invalides
- Vérifier que les variables d'environnement contiennent des données valides :
  - `idBible` : utiliser une bible disponible (LSG, OST, SBLGNT, WLC, etc.)
  - `idBook` : utiliser un identifiant de livre valide
  - `searchWords` : utiliser un mot pertinent pour la recherche

## Notes

- Tous les tests fonctionnent uniquement en **lecture (GET)**
- Les tests validez la structure JSON et les codes HTTP
- L'ordre des requêtes suit une hiérarchie : Bible → Book → Chapter → Verse(s)
- Chaque catégorie (Health, Search, Explore, Compare) peut être testée indépendamment