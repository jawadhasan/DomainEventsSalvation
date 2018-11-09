using System;

namespace ExampleCode.DelegatesAndEvents.DelegateCode
{
    public class MediaInventory
    {
        public delegate bool TestMedia();

        public void TestResult(TestMedia mediaDelegate)
        {
            Console.WriteLine(mediaDelegate() ? 
                "Media Ok, Add to inventory" : "Unable to play. Rejected");
        }
    }


    public class RecordPlayer
    {
        public bool PlayRecord()
        {
            //replace with actual testing
            Console.WriteLine("Testing....works");
            return true;
        }
    }

    public class CassettePlayer
    {
        public bool PlayCassette()
        {
            //replace with actual testing
            Console.WriteLine("Testing....Failed");
            return false;
        }
    }

    public class Worker
    {
        public Worker()
        {
            Work();
        }
        public void Work()
        {
            var mediaInventory = new MediaInventory();
            var recordPlayer = new RecordPlayer();
            var cassettePlayer = new CassettePlayer();

            var testRecordDelegate = new MediaInventory.TestMedia(recordPlayer.PlayRecord);
            var testCassetteDelegate = new MediaInventory.TestMedia(cassettePlayer.PlayCassette);

            mediaInventory.TestResult(testRecordDelegate);
            mediaInventory.TestResult(testCassetteDelegate);
        }
    }
}
