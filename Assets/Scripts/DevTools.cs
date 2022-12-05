using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DevTools : MonoBehaviour
{
    PlayerInputs playerControls;
    private InputAction teleport;
    private InputAction spawnBarrel;
    private InputAction resetScene;

    public GameObject teleportObject;
    public GameObject spawnObject;

    private Vector2 mousePos;


    private void Awake()
    {
        playerControls = new PlayerInputs();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    private void OnEnable()
    {
        teleport = playerControls.Player.DevToolTeleport;
        spawnBarrel = playerControls.Player.DevToolSpawnBarrel;
        resetScene = playerControls.Player.DevToolResetScene;


        teleport.Enable();
        spawnBarrel.Enable();
        resetScene.Enable();


        teleport.performed += TeleportPlayer;
        spawnBarrel.performed += SpawnBarrelAtMouse;
        resetScene.performed += ResetScene;
    }

    private void OnDisable()
    {
        teleport.Disable();
        spawnBarrel.Disable();
        resetScene.Disable();
    }

    private void TeleportPlayer(InputAction.CallbackContext context)
    {
        teleportObject.transform.position = mousePos;
    }
    private void SpawnBarrelAtMouse(InputAction.CallbackContext context)
    {
        Instantiate(spawnObject, mousePos, Quaternion.identity);
    }
    private void ResetScene(InputAction.CallbackContext context)
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
