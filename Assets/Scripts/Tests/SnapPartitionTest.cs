using System;
using UnityEngine;

namespace snaptest
{
    public class SnapPartitionTest : SnapTestRunner
    {
        public override SnapUnitTest[] UnitTests
        {
            get
            {
                return new SnapUnitTest[] {
                    testSnapPartitionSource,
                    testSnapPartitionTarget
                };
            }
        }

        public void testSnapPartitionSource()
        {
            SnapPartion partition = SnapPartion.Partition(childA);

            AssertTrue(partition.sources.Length == 1 && partition.sources[0] == snapA);
        }

        public void testSnapPartitionTarget()
        {
            SnapPartion partition = SnapPartion.Partition(childA);

            AssertTrue(partition.targets.Length == 1 && partition.targets[0] == snapB);
        }
    }
}
