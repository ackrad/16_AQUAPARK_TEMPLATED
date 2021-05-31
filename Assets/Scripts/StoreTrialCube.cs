using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTrialCube : MonoBehaviour
{
    private StoreController _storeController;

    private MeshRenderer _meshRenderer;

    public float rotateSpeed = 100f;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        transform.RotateAround(transform.position,Vector3.up,Time.deltaTime*rotateSpeed);
    }
    
    
    // Start is called before the first frame update
    private void Start()
    {
        _storeController = StoreController.request();
        storeItemSelected(StoreItemType.Material,null);
        _storeController.onItemSelected.AddListener(storeItemSelected);

    }

    private void storeItemSelected(StoreItemType type, StoreItem item)
    {
        Material selectedMaterial = (Material) _storeController.getSelectedItemOfType(StoreItemType.Material).Item;
        Texture selectedTexture =(Texture) _storeController.getSelectedItemOfType(StoreItemType.Texture).Item;
        selectedMaterial.mainTexture = selectedTexture;
        _meshRenderer.material = selectedMaterial;
    }

}
