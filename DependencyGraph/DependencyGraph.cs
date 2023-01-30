/// <summary>
/// Author:    Sora Roberts
/// Partner:   None
/// Date:      1/30/23
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Sora Roberts - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Sora Roberts, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source,
/// minus any necessary files directly from the professor's Canvas page.   
/// All references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///
///    This library is a reference for future programs that will
///    need the ability to track, graph, traverse, and otherwise analyze nodes
///    which depend on eachother. It's a very bare-bones dependency graph!
/// </summary>


// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SpreadsheetUtilities
{
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
  public class DependencyGraph
    {
        // Fields: two mirrored dictionaries. When a string representing a node is given as a key,
        // they correspond to strings that represent other nodes in the hashsets as required in methods below.
        private Dictionary<string, HashSet<string>> dependents;
        private Dictionary<string, HashSet<string>> dependees;


        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            // Instantiate the two (still empty) mirrored dictionaries.
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get {
                int count = 0;
                foreach (HashSet<string> set in dependents.Values)
                    count += set.Count();
                return count;
            }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (!dependees.ContainsKey(s))
                    return 0;
                return dependees[s].Count();
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty. O(1).
        /// </summary>
        public bool HasDependents(string s)
        {
            if (!dependents.ContainsKey(s))
                return false;
            if (dependents[s].Count == 0)
                return false;
            else return true;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty. O(1).
        /// </summary>
        public bool HasDependees(string s)
        {
            if (!dependees.ContainsKey(s))
                return false;
            if (dependees[s].Count == 0)
                return false;
            else return true;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (!dependents.ContainsKey(s))
                return new string[0];
            return CopyHashSet(dependents[s]);
        }


        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (!dependees.ContainsKey(s))
                return new string[0];
            return CopyHashSet(dependees[s]); 
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist. O(1).</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            TryMirroredEdit(true, s, t);
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists. O(1).
        /// </summary>
        /// <param name="s"> being removed as t's dependee</param>
        /// <param name="t"> being removed as s's dependent</param>
        public void RemoveDependency(string s, string t)
        {
            TryMirroredEdit(false, s, t);
            
            // delete the each node if they are no longer connected to the graph
            TryDeleteNode(s);
            TryDeleteNode(t);
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (dependents.ContainsKey(s))
            {
                foreach (string r in dependents[s])
                    RemoveDependency(s, r);
            }

            foreach (string t in newDependents)
                AddDependency(s, t);
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (dependees.ContainsKey(s))
            {
                foreach (string r in dependees[s])
                    RemoveDependency(r, s);
            }

            foreach (string t in newDependees)
                AddDependency(t, s);
        }


        /// <summary>
        /// A helper method which tries to perform operations on the mirrored dictionaries;
        /// either adding or removing a dependency. If the operation cannot be performed
        /// (ex, failing to add a duplicate dependency or failing to remove a nonexistant one),
        /// then the dictionaries will not be impacted and no errors will be thrown.
        /// </summary>
        /// <param name="isAdd">true if a dependency is being added, false if being removed.</param>
        /// <param name="s">the dependee to be added or removed, if applicable</param>
        /// <param name="t">the dependent to be added or removed, if applicable</param>
        /// <returns>true if the operation was completed, false if there was no impact on the mirrored dictionaries.</returns>
        private Boolean TryMirroredEdit(Boolean isAdd, string s, string t)
        {
            //if (s == null || t == null) return false; // don't add or remove null nodes
            // ^^ NOT REQUIRED per piazza.

            if (!isNodeEstablished(isAdd, s)) // we're trying to remove a non-existant dependency
                return false;
            if (!isNodeEstablished(isAdd, t)) // we're trying to remove a non-existant dependency
                return false;
            // s and r exist as nodes (possibly with established dependents/dependees, possibly not)

            if (!isAdd && dependents[s].Contains(t)) // if (s,r) exists and we're removing it
            {
                dependents[s].Remove(t);
                dependees[t].Remove(s);
                return true;
            }
            if (isAdd && !dependents[s].Contains(t)) // if (s,r) doesn't exist and we're adding it
            {
                dependents[s].Add(t);
                dependees[t].Add(s);
                return true;
            }
            // operation couldn't happen
            return false;
        }


        /// <summary>
        /// A helper method that establishes a node if needed to add to a dependency,
        /// (does not change establishment if intention is to remove a dependency),
        /// and then tells the user if the node is established.
        /// </summary>
        /// <param name="isAdd">true if a dependency is being added, false if being removed.</param>
        /// <param name="node">The node to be established in the dictionaries.</param>
        /// <returns>true if the node is now established and ready to add,
        /// false if the intention is not to add and no node was established.</returns>
        private Boolean isNodeEstablished(Boolean isAdd, string node) 
        {
            if (!dependents.ContainsKey(node)) // if node is not established yet
            {
                if (isAdd) // add node to the dictionaries so we can add a dependency
                {
                    dependents.Add(node, new HashSet<string>());
                    dependees.Add(node, new HashSet<string>());
                }
                else // we're trying to remove a dependency that doesn't exist
                    return false;
            }
            return true; // the node is established, ready to add to or remove
        }


        /// <summary>
        /// A helper method that deletes nodes if they aren't connected to any dependencies
        /// as either a dependent or a dependee (AKA, if they are independent/isolated completely).
        /// It does nothing otherwise
        /// </summary>
        /// <param name="node">The node to be checked and possibly deleted</param>
        private void TryDeleteNode(string node)
        {
            if (dependees.ContainsKey(node)) //if the node exists...
            {
                // ... and if the node isn't connected to any dependencies,
                if (dependees[node].Count() == 0 &&
                    dependents[node].Count() == 0)
                {
                    // ... remove the node as a key from both dictionaries
                    dependees.Remove(node);
                    dependents.Remove(node);
                }
            }
        }


        /// <summary>
        /// A helper method that copies the contents of a HashSet into a new array of the appropriate size
        /// without editing the HashSet.
        /// </summary>
        /// <param name="set">The hashset to be duplicated</param>
        /// <returns>The array with the original hashset's contents</returns>
        private string[] CopyHashSet(HashSet<string> set)
        {
            // initialize an array the size of the hashset of dependees of s
            int dictionaryCount = set.Count();
            string[] copyArray = new string[dictionaryCount];

            // copy the hashset to the array, return the array
            set.CopyTo(copyArray);
            return copyArray;
        }
    }
}