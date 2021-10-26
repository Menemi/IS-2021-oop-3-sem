using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IsuExtra
{
    public enum MegaFaculty
    {
        TINT = 0,
        KTU,
        FT,
        BTINS,
        FTMI,
    }

    public class JointGroupOfTrainingDirections
    {
        private MegaFaculty _megaFaculty;

        private List<Stream> _streams;

        public JointGroupOfTrainingDirections(MegaFaculty megaFaculty, List<Stream> streams)
        {
            _megaFaculty = megaFaculty;
            _streams = streams;
        }

        public MegaFaculty GetMegaFaculty()
        {
            return _megaFaculty;
        }

        public ReadOnlyCollection<Stream> GetStreams()
        {
            return _streams.AsReadOnly();
        }
    }
}