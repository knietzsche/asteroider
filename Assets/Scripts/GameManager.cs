using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameState gameState { get; private set; } = GameState.mainmenu;

    [SerializeField] private Transform _gameBoard = null;
    [SerializeField] private Ship _shipPrefab = null;
    [SerializeField] private Asteroid _asteroidPrefab = null;
    [SerializeField] private Shootable _bonusPrefab = null;
    [SerializeField] private Shootable _enemyPrefab = null;

    [Header("Game Settings")]
    [SerializeField] private int _asteroidInitialCount = 1;
    [Range(0f, 5f)]
    [SerializeField] private float _safeSpawnDistance = 4f;
    [SerializeField] private int _lifeMax = 3;
    [SerializeField] private float _respawnDelay = 3f;

    [SerializeField] float _specialSpawnDelayInitial = 30f;
    [Range(0f, 1f)]
    [SerializeField] float _specialSpawnRatio = .8f;

    float _specialProbability;
    float _specialSpawnDelay;
    
    private int _life;
    private int _score;
    private int _asteroidCount;

    private Coroutine _specialSpawner;
    private Coroutine _shipSpawner;
    private Coroutine _asteroidRespawner;

    private void Awake()
    {
        UserAction.ChangeScreen += OnChangeScreen;
        UserAction.PauseGame += OnPauseGame;

        GameAction.ShipDestroyed += OnShipDestroyed;
        GameAction.SpecialDestroyed += OnSpecialDestroyed;
        GameAction.UpdateAsteroidCount += OnUpdateAsteroidCount;
        GameAction.AddScore += AddScore;
    }

    private void Start()
    {
        UserAction.ChangeScreen(ScreenManager.ScreenType.Menu);
    }

    private void OnChangeScreen(ScreenManager.ScreenType screenType)
    {
        if (screenType == ScreenManager.ScreenType.Menu)
        {
            gameState = GameState.mainmenu;
            ClearGameBoard();
            if (_specialSpawner != null)
            {
                StopCoroutine(_specialSpawner);
                _specialSpawner = null;
            }
            if (_shipSpawner != null)
            {
                StopCoroutine(_shipSpawner);
                _shipSpawner = null;
            }
            if (_asteroidRespawner != null)
            {
                StopCoroutine(_asteroidRespawner);
                _asteroidRespawner = null;
            }
            RespawnAll(true);
        }
        if (screenType == ScreenManager.ScreenType.Game)
        {
            ClearGameBoard();
            StartCoroutine(StartGameWait());
        }
    }

    private void OnPauseGame()
    {
        if (gameState == GameState.running)
        {
            gameState = GameState.paused;
            Time.timeScale = 0f;
            InterfaceAction.UpdateGamePaused?.Invoke(true);
        }
        else if (gameState == GameState.paused)
        {
            gameState = GameState.running;
            Time.timeScale = 1f;
            InterfaceAction.UpdateGamePaused?.Invoke(false);
        }
    }

    private void RespawnAll(bool demo = false)
    {
        for (int i = 0; i < _asteroidInitialCount; i++)
        {
            Instantiate(
                _asteroidPrefab,
                Viewport.GetRandomPositionInside(_safeSpawnDistance),
                Quaternion.identity,
                _gameBoard);
        }

        if (!demo)
        {
            SpawnShip();

            _specialProbability = 1f;
            _specialSpawnDelay = _specialSpawnDelayInitial;
            
            var prefab = _bonusPrefab;
            var position = Viewport.GetRandomPositionOnEdge();
            var seconds = _specialSpawnDelayInitial;

            _specialSpawner = StartCoroutine(SpawnSpecialWait(prefab, position, seconds));
        }
    }

    private void InitializePlayer()
    {
        InterfaceAction.ClearGameScreen?.Invoke();
        _life = _lifeMax;
        _score = 0;
        InterfaceAction.UpdateLife?.Invoke(_life);
        InterfaceAction.UpdateScore?.Invoke(_score);
    }

    private void OnUpdateAsteroidCount(bool value)
    {
        _asteroidCount += value ? 1 : -1;

        if (_asteroidCount == 0 && gameState == GameState.running)
        {
            _asteroidRespawner = StartCoroutine(RespawnAstreroidsWait());
        }
    }

    private IEnumerator RespawnAstreroidsWait()
    {
        yield return new WaitForSeconds(_respawnDelay);

        RespawnAll(true);
        _asteroidRespawner = null;
    }

    private void AddScore(int value)
    {
        _score += value;
        InterfaceAction.UpdateScore?.Invoke(_score);
    }

    private void OnShipDestroyed()
    {
        if (gameState != GameState.running)
        {
            return;
        }
        _life -= 1;
        InterfaceAction.UpdateLife?.Invoke(_life);

        if (_life > 0)
        {
            _shipSpawner = StartCoroutine(SpawnShipWait());
        }
        else
        {
            EndGame();
        }
    }

    private void OnSpecialDestroyed()
    {
        _specialProbability *= _specialSpawnRatio;
        var prefab = (Random.Range(0f, 1f) > _specialProbability) ?
            _enemyPrefab : _bonusPrefab;
        if (_specialSpawnDelay > 1f)
        {
            _specialSpawnDelay *= _specialSpawnRatio;
        }
        var position = Viewport.GetRandomPositionOnEdge();

        if (gameState == GameState.running)
        {
            _specialSpawner = StartCoroutine(SpawnSpecialWait(prefab, position, _specialSpawnDelay));
        }
    }

    private IEnumerator SpawnSpecialWait(GameboardObject prefab, Vector3 position, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Instantiate(
            prefab,
            position,
            Quaternion.identity,
            _gameBoard);

        _specialSpawner = null;
    }

    private IEnumerator SpawnShipWait()
    {
        yield return new WaitForSeconds(_respawnDelay);

        SpawnShip();
        _shipSpawner = null;
    }

    private void SpawnShip()
    {
        var ship = Instantiate(_shipPrefab, _gameBoard);
        ship.Initialize();
    }

    private void EndGame()
    {
        gameState = GameState.gameover;
        InterfaceAction.UpdateGameOver?.Invoke();
        if (_specialSpawner != null)
        {
            StopCoroutine(_specialSpawner);
            _specialSpawner = null;
        }
        StartCoroutine(EndGameWait());
    }

    private IEnumerator StartGameWait()
    {
        yield return new WaitForEndOfFrame();
        gameState = GameState.running;

        RespawnAll();
        InitializePlayer();
    }

    private IEnumerator EndGameWait()
    {
        yield return new WaitForSeconds(2f);

        gameState = GameState.ended;
    }

    private void ClearGameBoard()
    {
        for (int index = _gameBoard.childCount - 1; index >= 0; index--)
        {
            var child = _gameBoard.GetChild(index);
            Destroy(child.gameObject);
        }
    }

    private void OnValidate()
    {
        Debug.Assert(_gameBoard != null);
        Debug.Assert(_shipPrefab != null);
        Debug.Assert(_asteroidPrefab != null);
        Debug.Assert(_bonusPrefab != null);
        Debug.Assert(_enemyPrefab != null);
        Debug.Assert(_asteroidInitialCount > 0);
    }

    public enum GameState
    {
        mainmenu, running, paused, gameover, ended
    }
}
