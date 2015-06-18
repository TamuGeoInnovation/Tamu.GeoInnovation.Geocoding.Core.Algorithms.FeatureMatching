using System.Collections.Generic;
using USC.GISResearchLab.Common.Addresses;
using USC.GISResearchLab.Geocoding.Core.Queries.Parameters;
using USC.GISResearchLab.Geocoding.Core.ReferenceDatasets.ReferenceSourceQueries;

namespace USC.GISResearchLab.Geocoding.Core.ReferenceDatasets.Sources.Interfaces
{
    public interface IRelaxableMatcher
    {
        List<AddressComponents> RelaxableAttributesAllowable
        {
            get;
        }

        ReferenceSourceQuery[] BuildRelaxedQueries(ParameterSet parameterSet);
    }
}
