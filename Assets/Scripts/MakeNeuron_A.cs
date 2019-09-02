using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MakeNeuron_A : MonoBehaviour
{
    static Material lineMaterial;

    TreeNode<NodeItem> neuron;

    //--------------------------------------------
    //  Start is called before the first frame update
    //--------------------------------------------
    void Start()
    {
        neuron = CreateNeuronRecursive();
    }

    //--------------------------------------------
    //  Just start calling the recursive function. Makes code cleaner...
    //--------------------------------------------
    TreeNode<NodeItem> CreateNeuronRecursive()
    {

        //--- OjO: Start the tree here with origin at zeros
        Vector3 vec_origin = new Vector3(0f, 0f, 0f);
        TreeNode<NodeItem> root = new TreeNode<NodeItem>(new NodeItem(0, vec_origin, vec_origin));

        //--- Recursively fill tree. Pass root node, its position and number of levels
        int max_levels = 7;
        fillRecursive(root, vec_origin, max_levels);

        return root;
    }

    //===========================================================================
    //  Recursive function for adding dendrites to neuron. 
    //  OjO: Here is where we define how the neuron will look like.
    //===========================================================================
    void fillRecursive(TreeNode<NodeItem> _node, Vector3 vec_parent, int max_levels)
    {

        //--- Escape condition. Make sure we leave recursion!
        if (_node.Level > max_levels)
        {
            return;
        }

        int lim1, lim2;

        if (_node.Level == 0) //--- Start with many dendrites connected to neuron
        {
            lim1 = 4;
            lim2 = 8;
        }
        else  //--- Dendrites have low number of branches
        {
            lim1 = 1;
            lim2 = 3;
        }

        //--- Add children branches to this node
        for (int i = 0; i < Random.Range(lim1, lim2); i++)
        {

            //--- Branch lenght range. TWEAK
            float len1 = (_node.Level - (max_levels - 1)) * 3f;
            float len2 = len1 + 2f;

            //--- How aligned is this branch with its parent?
            float align_thres = 0.8f;

            Vector3 vec_i = get_directed_random(vec_parent, align_thres, len1, len2);

            TreeNode<NodeItem> child_i = _node.AddChild(new NodeItem(i, vec_parent, vec_parent + vec_i));
            fillRecursive(child_i, vec_parent + vec_i, max_levels);
        } // for i

    }


    //--------------------------------------------
    // Will be called after all regular rendering is done
    //--------------------------------------------
    public void OnRenderObject()
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        renderNeuron();
    }

    //--------------------------------------------
    // Good old OpenGL direct mode...
    //--------------------------------------------
    void renderNeuron()
    {

        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);

        // Draw lines
        GL.Color(new Color(0.2f, 0.2f, 1.0f, 0.8f));
        GL.Begin(GL.LINES);

        //--- Here is where we extract all the nodels in the tree
        foreach (TreeNode<NodeItem> node in neuron)
        {
            GL.Vertex3(node.Data.PAR.x, node.Data.PAR.y, node.Data.PAR.z);
            GL.Vertex3(node.Data.VEC.x, node.Data.VEC.y, node.Data.VEC.z);
        }
        GL.End();
        GL.PopMatrix();
    }

    //--------------------------------------------
    //  Get a random position on the sphere over a range in radius
    //--------------------------------------------
    Vector3 get_random(float _low, float _high)
    {

        float _r = Random.Range(_low, _high);
        float _alpha = Random.Range(0f, 2f * Mathf.PI);
        float _beta = Random.Range(-Mathf.PI / 2f, Mathf.PI / 2f);
        float _x = _r * Mathf.Cos(_beta) * Mathf.Cos(_alpha);
        float _y = _r * Mathf.Cos(_beta) * Mathf.Sin(_alpha);
        float _z = _r * Mathf.Sin(_beta);

        return new Vector3(_x, _y, _z);
    }

    //--------------------------------------------
    //  Get a semi-random position on the sphere over a range in radius and angle form a reference vector
    //  I do this in a while loop which is very inneficient. It should be done correctly...
    //--------------------------------------------
    Vector3 get_directed_random(Vector3 parent, float thres, float _low, float _high)
    {

        Vector3 vec = new Vector3(0f, 0f, 0f);
        float dot = 0;
        int cont = 0;
        while (dot < thres)
        {
            vec = get_random(_low, _high);
            dot = Vector3.Dot(parent.normalized, vec.normalized);

            //--- Avoid staying here forever...
            cont++;
            if (cont > 100) dot = 1;
        }

        return vec;
    }

    //--------------------------------------------
    //
    //--------------------------------------------
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Built-in shader
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
        }
    }

}