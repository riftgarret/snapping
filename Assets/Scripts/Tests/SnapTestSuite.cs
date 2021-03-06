// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace snaptest
{
    public class SnapTestSuite
    {        

        public delegate void SnapUnitTest();

        Type[] testClasses = new Type[] {
            typeof(SnapDatabaseTest),
            typeof(SnapPartitionTest)
        };


        public void testAll()
        {
            try
            {
                foreach(Type type in testClasses)
                {
                    SnapTestRunner runner = (SnapTestRunner) Activator.CreateInstance(type);
                    
                    foreach(SnapTestRunner.SnapUnitTest testMethod in runner.UnitTests)
                    {
                        test(runner, testMethod);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void test(SnapTestRunner runner, SnapTestRunner.SnapUnitTest testMethod)
        {
            try
            {
                runner.setUp();
                testMethod();
                LogSuccess(testMethod);
            }
            catch (Exception e)
            {
                LogFailure(testMethod, e);
            }
            finally
            {
                runner.tearDown();
            }
        }

        // assert code
        void LogSuccess(SnapTestRunner.SnapUnitTest testMethod)
        {
            Debug.Log(testMethod.Method.Name + " SUCCESS");
        }

        void LogFailure(SnapTestRunner.SnapUnitTest testMethod, Exception exception)
        {
            Debug.LogError(testMethod.Method.Name + " FAILURE " + exception.Message);
        }
                     
    }

    

    public class SnapAssertException : Exception
    {
        public SnapAssertException(string msg) : base(msg) { }
    }
}

