using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;
using static GameManager;

public class GunEquipManager : MonoBehaviour
{
    [Header("Meta Controllers")]
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;

    [Header("Gun Mount Points")]
    [SerializeField] private Transform leftHandMount;
    [SerializeField] private Transform rightHandMount;

    [Header("Gun Prefabs")]
    [SerializeField] private GameObject[] gunPrefabs;

    private GameObject currentLeftGun;
    private GameObject currentRightGun;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        GameManager.Instance.OnGunChanged += GameManager_OnGunChanged;

        // Equip initial guns
        EquipGuns(Gun.GunType.Type1);
        ToggleControllers(false);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        GameManager.Instance.OnGunChanged += GameManager_OnGunChanged;
    }

    private void GameManager_OnGunChanged(Gun.GunType gunType)
    {
        UnEquipGuns();
        EquipGuns(gunType);
    }

    private void GameManager_OnGameStateChanged(GameManager.GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                ToggleGuns(true);
                ToggleControllers(false);
                break;
            case GameState.Paused:
                ToggleGuns(false);
                ToggleControllers(true);
                break;
            default: break;
        }
    }

    private void EquipGuns(Gun.GunType gunType)
    {
        int gunTypeIndex = (int)gunType;

        // Equip left and right guns
        currentLeftGun = Instantiate(gunPrefabs[gunTypeIndex], leftHandMount);
        currentLeftGun.GetComponent<Gun>().SetGunHand(OVRInput.Controller.LTouch, OVRInput.Axis1D.PrimaryIndexTrigger);

        currentRightGun = Instantiate(gunPrefabs[gunTypeIndex], rightHandMount);
        currentRightGun.GetComponent<Gun>().SetGunHand(OVRInput.Controller.RTouch, OVRInput.Axis1D.SecondaryIndexTrigger);
    }

    private void UnEquipGuns()
    {
        if (currentLeftGun != null) Destroy(currentLeftGun);
        if (currentRightGun != null) Destroy(currentRightGun);
    }

    private void ToggleGuns(bool visible)
    {
        if (currentLeftGun != null) currentLeftGun.SetActive(visible);
        if (currentRightGun != null) currentRightGun.SetActive(visible);
    }

    private void ToggleControllers(bool enable)
    {
        leftController.SetActive(enable);
        rightController.SetActive(enable);
    }
}
