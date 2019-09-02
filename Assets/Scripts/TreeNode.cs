using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//=================================================================
// We use this class to store useful information for each node it the tree.
// Eventually it should contain some methods
//=================================================================
class NodeItem
{
    private double _Id;   //--- Node index in its level
    private Vector3 vec;  //--- This vector
    private Vector3 par;  //--- Parent's position

    //--- Constructor
    public NodeItem(int Id, Vector3 _par, Vector3 _vec)
    {
        this._Id = Id;
        this.vec = _vec;
        this.par = _par;
    }

    //=== Accesors:

    //--- Node's Id, we don't use them for now...
    public double ID
    {
        get { return this._Id; }
        set { this._Id = value; }
    }

    //--- Node's position
    public Vector3 VEC
    {
        get { return this.vec; }
        set { this.vec = value; }
    }

    //--- Parent node's position. Wasteful but easy, needed to graw vectors from parent to this node.
    public Vector3 PAR
    {
        get { return this.par; }
        set { this.par = value; }
    }
}

//=================================================================
// Code copied from:
// https://github.com/gt4dev/yet-another-tree-structure
// see also:
// https://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp/2012855#2012855
//=================================================================
public class TreeNode<T> : IEnumerable<TreeNode<T>>
{

    public T Data { get; set; }
    public TreeNode<T> Parent { get; set; }
    public ICollection<TreeNode<T>> Children { get; set; }

    //--------------------
    //
    //--------------------
    public bool IsRoot
    {
        get { return Parent == null; }
    }

    //--------------------
    //
    //--------------------
    public bool IsLeaf
    {
        get { return Children.Count == 0; }
    }

    //--------------------
    //
    //--------------------
    public int Level
    {
        get
        {
            if (this.IsRoot)
                return 0;
            return Parent.Level + 1;
        }
    }

    //--------------------
    //
    //--------------------
    public TreeNode(T data)
    {
        this.Data = data;
        this.Children = new LinkedList<TreeNode<T>>();
    }

    //--------------------
    //
    //--------------------
    public TreeNode<T> AddChild(T child)
    {
        TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
        this.Children.Add(childNode);
        return childNode;
    }

    //--------------------
    //
    //--------------------
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    //--------------------
    //
    //--------------------
    public IEnumerator<TreeNode<T>> GetEnumerator()
    {
        yield return this;
        foreach (var directChild in this.Children)
        {
            foreach (var anyChild in directChild)
                yield return anyChild;
        }
    }

}