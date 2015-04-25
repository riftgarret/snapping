using UnityEngine;
using UnityEditor;
using System;

namespace snaptest
{
    

    public abstract class SnapTestRunner
    {
        protected GameObject rootGO;

        protected GameObject childA;
        protected GameObject childB;

        protected SnappableComponent snapA;
        protected SnappableComponent snapB;

        public delegate void SnapUnitTest();

        public abstract SnapUnitTest[] UnitTests
        {
            get;
        }


        internal void setUp()
        {
            rootGO = new GameObject();
            rootGO.name = "rootGO";
            childA = createGameObjectWithSnappable(rootGO);
            childA.name = "childA";
            childB = createGameObjectWithSnappable(rootGO);
            childB.name = "childB";
            snapA = childA.GetComponentInChildren<SnappableComponent>();
            snapA.gameObject.name = "snapA";
            snapB = childB.GetComponentInChildren<SnappableComponent>();
            snapB.gameObject.name = "snapB";
        }

        internal void tearDown()
        {
            GameObject.DestroyImmediate(rootGO);
            rootGO = null;
            childA = null;
            childB = null;
            snapA = null;
            snapB = null;
        }

        

        private GameObject createGameObjectWithSnappable(GameObject parent)
        {
            GameObject child = new GameObject();
            GameObject snapChild = new GameObject();
            //snapChild.transform.Translate(10000, 10000, 10000); // move out of center for testing        
            snapChild.AddComponent<SnappableComponent>();
            snapChild.transform.SetParent(child.transform);
            child.transform.SetParent(parent.transform);
            return child;
        }


        protected void Assert(object expected, object result)
        {
            if (!expected.Equals(result))
            {
                throw new SnapAssertException("expected: " + expected + " actual: " + result);
            }
        }

        protected void AssertNull(object expectedNull)
        {
            if (expectedNull != null)
            {
                throw new SnapAssertException("expected: null actual: " + expectedNull);
            }
        }

        protected void AssertTrue(bool expectedTrue)
        {
            if (!expectedTrue)
            {
                throw new SnapAssertException("expected: true actual: false");
            }
        }

        public class SnapAssertException : Exception
        {
            public SnapAssertException(string msg) : base(msg) { }
        }
    }
}
