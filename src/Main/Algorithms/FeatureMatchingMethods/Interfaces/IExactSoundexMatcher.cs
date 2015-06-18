using USC.GISResearchLab.Geocoding.Core.Queries.Parameters;
using USC.GISResearchLab.Geocoding.Core.ReferenceDatasets.ReferenceSourceQueries;

namespace USC.GISResearchLab.Geocoding.Core.ReferenceDatasets.Sources.Interfaces
{
    public interface IExactSoundexMatcher : ISoundexMatcher
    {
        ReferenceSourceQuery[] BuildExactSoundexQueries(ParameterSet parameterSet);
    }
}
