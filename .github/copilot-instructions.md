# Croupier - Card Deck API
Croupier is a .NET 6.0 ASP.NET Core minimal API that serves playing cards from a deck. It provides endpoints to create game sessions, draw cards, view deck contents, and shuffle decks. The application includes Swagger UI and supports multi-deck games.

**ALWAYS reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Working Effectively

### Prerequisites and Installation
- Install .NET 6.0 SDK - REQUIRED (Ubuntu 24.04 only has .NET 8.0 by default):
  - `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 6.0`
  - `export PATH="/home/runner/.dotnet:$PATH"`
  - `export DOTNET_ROOT="/home/runner/.dotnet"`
  - Verify: `dotnet --version` (should show 6.0.x)

### Build and Test Commands
**NEVER CANCEL builds or tests - wait for completion. Set timeouts appropriately.**

- Restore dependencies: `dotnet restore` (15 seconds)
- Build Debug: `dotnet build --configuration Debug` (2 seconds after restore)
- Build Release: `dotnet build --configuration Release` (9 seconds with warnings)
- Run tests: `dotnet test` (7 seconds - **NOTE: 1 test fails but 8 pass**)
- Publish: `dotnet publish -c Release -o /tmp/publish` (2 seconds)

**CRITICAL TIMEOUTS:**
- Build commands: Set timeout to 120+ seconds minimum
- Test commands: Set timeout to 60+ seconds minimum
- NEVER CANCEL operations that appear to hang - they may be legitimate

### Run the Application

#### Development Mode
- From Croupier.App directory: `dotnet run`
- Listens on: https://localhost:7279 and http://localhost:5050
- Swagger UI: https://localhost:7279/ (root path)

#### Production Mode (Published)
- From published directory: `./Croupier`
- Listens on: https://localhost:5001 and http://localhost:5000
- Use HTTP endpoint for testing: http://localhost:5000

### Code Quality and Formatting
- Format code: `dotnet format` (auto-fixes spacing/formatting issues)
- Check format: `dotnet format --verify-no-changes --verbosity diagnostic`
- **WARNING: Code has formatting issues** - always run `dotnet format` before committing

## Validation and Testing

### ALWAYS run this complete end-to-end validation scenario after making changes:
```bash
# 1. Create new game
GAME_ID=$(curl -s http://localhost:5000/new-game)
echo "Game ID: $GAME_ID"

# 2. Verify full deck (52 cards)
DECK_COUNT=$(curl -s "http://localhost:5000/see-deck?sessionId=$GAME_ID" | jq length)
echo "Deck has $DECK_COUNT cards" # Should be 52

# 3. Draw cards and verify deck reduces
curl -s "http://localhost:5000/draw-card?sessionId=$GAME_ID" | jq '.code'
DECK_COUNT=$(curl -s "http://localhost:5000/see-deck?sessionId=$GAME_ID" | jq length)
echo "Deck now has $DECK_COUNT cards" # Should be 51

# 4. Test shuffle functionality
curl -s "http://localhost:5000/shuffle-deck?sessionId=$GAME_ID"

# 5. Test multi-deck (3 decks = 156 cards)
GAME_ID2=$(curl -s "http://localhost:5000/new-game?numberOfDecks=3")
DECK_COUNT2=$(curl -s "http://localhost:5000/see-deck?sessionId=$GAME_ID2" | jq length)
echo "Multi-deck game has $DECK_COUNT2 cards" # Should be 156
```

### API Endpoints
- `GET /new-game?numberOfDecks={n}` - Create session, returns session ID
- `GET /see-deck?sessionId={id}` - View all cards in deck
- `GET /draw-card?sessionId={id}` - Draw top card from deck
- `GET /shuffle-deck?sessionId={id}` - Shuffle remaining cards

## Known Issues and Workarounds
- **1 failing test**: `DrawingAllCardsEmptiesDeck` fails but other 8 tests pass
- **Code formatting**: Multiple whitespace formatting issues exist - run `dotnet format` to fix
- **.NET 6.0 dependency**: Must install .NET 6.0 SDK manually on Ubuntu 24.04
- **Nullable reference warning**: CS8602 warning in tests (line 102) but does not break build

## Deployment and CI/CD
- GitHub Actions workflow: `.github/workflows/main_croupier.yml`
- Targets Azure Web App deployment
- Uses Windows runner and .NET 6.0.x
- Build and publish steps match local commands

## Repository Structure
```
Croupier.App/               # Main API application
├── Endpoints/              # API endpoint definitions
├── Model/                  # Card, Deck, Session models  
├── Services/               # ID generation service
├── Workers/                # Initializer, SessionManager, Shuffler
└── Program.cs              # Application entry point

Croupier.Tests/             # xUnit test project
├── CroupierTests.cs        # API integration tests
└── TestModel/              # Test model classes

.github/workflows/          # CI/CD configuration
```

## Common Tasks
- **Add new endpoint**: Create in `Endpoints/` folder, implement `IEndpoint`, register in `DependencyInitializer.cs`
- **Modify card logic**: Check `Workers/Initializer.cs` for deck creation, `Workers/Shuffler.cs` for shuffling
- **Update models**: Core models in `Model/` folder (Card, Deck, Session)
- **Debug failing test**: See `CroupierTests.cs` line 148 (`DrawingAllCardsEmptiesDeck`)

Always build, test, format, and run the validation scenario before committing changes.