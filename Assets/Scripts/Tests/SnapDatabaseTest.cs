using System;
using UnityEngine;

namespace snaptest
{
    public class SnapDatabaseTest : SnapTestRunner
    {
        public override SnapUnitTest[] UnitTests
        {
            get
            {
                return new SnapUnitTest[] {
                    testSnapFacingEachother,
                    testSnapFacingSameDirection
                };
            }
        }

        public void testSnapFacingEachother()
        {
            childA.transform.Rotate(Vector3.up, 180);
            childA.transform.Translate(0.5f, 0.5f, 0.5f);

            SnapDatabase filter = new SnapDatabase();
            filter.AngleThreshold = 1;
            filter.DistanceThreshold = 2;

            filter.ResetTargets(new SnappableComponent[] { snapB });
            SnappableComponent[] expectedTarget = filter.GetTargets(snapA);

            Assert(snapB, expectedTarget[0]);
        }

        public void testSnapFacingSameDirection()
        {
            childA.transform.Translate(0.5f, 0.5f, 0.5f);

            SnapDatabase filter = new SnapDatabase();
            filter.AngleThreshold = 1;
            filter.DistanceThreshold = 2;

            filter.ResetTargets(new SnappableComponent[] { snapB });
            SnappableComponent[] expectedTarget = filter.GetTargets(snapA);

            Assert(0, expectedTarget.Length);
        }
    }
}
