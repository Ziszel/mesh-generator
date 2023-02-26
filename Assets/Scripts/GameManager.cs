using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button showGridButton;
    public Button showCubeButton;
    public Button showQuadButton;
    public Button showSphereButton;
    public Button generateButton;
    public QuadGenerator quad;
    public QuadGridGenerator quadGrid;
    public CubeGenerator cube;
    public SphereGenerator sphere;
    public Slider widthSlider;
    public Slider heightSlider;
    public Slider depthSlider;
    public Slider sizeSlider;
    private int _width;
    private int _height;
    private int _depth;
    private int _sphereSize;
    void Start()
    {
        _width = 1;
        _height = 1;
        _depth = 1;
        _sphereSize = 10;
        
        showGridButton.onClick.AddListener(ShowQuadGrid);
        showCubeButton.onClick.AddListener(ShowCube);
        showQuadButton.onClick.AddListener(ShowQuad);
        showSphereButton.onClick.AddListener(ShowSphere);
        generateButton.onClick.AddListener(CallGenerateMesh);
        widthSlider.onValueChanged.AddListener(OnWidthValueChange);
        heightSlider.onValueChanged.AddListener(OnHeightValueChange);
        depthSlider.onValueChanged.AddListener(OnDepthValueChange);
        sizeSlider.onValueChanged.AddListener(OnSizeValueChange);
        
        // No object is selected at first so set all of the sliders to false
        sizeSlider.gameObject.SetActive(false);
        widthSlider.gameObject.SetActive(false);
        heightSlider.gameObject.SetActive(false);
        depthSlider.gameObject.SetActive(false);
    }

    private void CallGenerateMesh()
    {
        // GenerateQuadGrid takes in 2 ints (width, and height)
        if (quadGrid.gameObject.activeInHierarchy)
        {
            quadGrid.GenerateQuadGrid(_width, _height);
        }

        // GenerateCube takes in three floats (width, height, and depth)
        if (cube.gameObject.activeInHierarchy)
        {
            cube.GenerateCube(_width, _height, _depth);
        }

        // Generate quad takes in two floats (width and height)
        if (quad.gameObject.activeInHierarchy)
        {
            quad.GenerateQuad(_width, _height);
        }

        // Initialise takes in an int
        if (sphere.gameObject.activeInHierarchy)
        {
            sphere.Initialise(_sphereSize);
        }
    }
    
    // These functions link the UI to the values that will be passed into the generation scripts of the meshes
    public void OnWidthValueChange(float value)
    {
        _width = (int)value;
    }
    
    public void OnHeightValueChange(float value)
    {
        _height = (int)value;
    }
    
    public void OnDepthValueChange(float value)
    {
        _depth = (int)value;
    }
          
    public void OnSizeValueChange(float value)
    {   
        _sphereSize = (int)value;
    }
    
    // All functions below related to UI specifically
    private void ShowQuad()
    {
        // show / hide the objects
        quad.gameObject.SetActive(true);
        quadGrid.gameObject.SetActive(false);
        cube.gameObject.SetActive(false);
        sphere.gameObject.SetActive(false);
        
        // show / hide the UI
        widthSlider.gameObject.SetActive(true);
        heightSlider.gameObject.SetActive(true);
        depthSlider.gameObject.SetActive(false);
        sizeSlider.gameObject.SetActive(false);
    }
    
    private void ShowQuadGrid()
    {
        // show / hide the objects
        quadGrid.gameObject.SetActive(true);
        quad.gameObject.SetActive(false);
        cube.gameObject.SetActive(false);
        sphere.gameObject.SetActive(false);
        
        // show / hide the UI
        widthSlider.gameObject.SetActive(true);
        heightSlider.gameObject.SetActive(true);
        depthSlider.gameObject.SetActive(false);
        sizeSlider.gameObject.SetActive(false);
    }
    
    private void ShowCube()
    {
        // show / hide the objects
        cube.gameObject.SetActive(true);
        quadGrid.gameObject.SetActive(false);
        quad.gameObject.SetActive(false);
        sphere.gameObject.SetActive(false);
        
        // show / hide the UI
        widthSlider.gameObject.SetActive(true);
        heightSlider.gameObject.SetActive(true);
        depthSlider.gameObject.SetActive(true);
        sizeSlider.gameObject.SetActive(false);
    }
    
    private void ShowSphere()
    {
        // show / hide the objects
        sphere.gameObject.SetActive(true);
        cube.gameObject.SetActive(false);
        quadGrid.gameObject.SetActive(false);
        quad.gameObject.SetActive(false);
        
        // show / hide the UI
        sizeSlider.gameObject.SetActive(true);
        widthSlider.gameObject.SetActive(false);
        heightSlider.gameObject.SetActive(false);
        depthSlider.gameObject.SetActive(false);
    }
}
