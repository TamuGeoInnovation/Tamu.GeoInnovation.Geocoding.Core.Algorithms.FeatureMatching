using System;
using USC.GISResearchLab.Common.Addresses;
using USC.GISResearchLab.Common.Core.Geocoders.FeatureMatching;
using USC.GISResearchLab.Geocoding.Core.Algorithms.FeatureMatchScorers;
using USC.GISResearchLab.Geocoding.Core.ReferenceDatasets.ReferenceFeatures;

namespace USC.GISResearchLab.Geocoding.Core.Algorithms.FeatureMatchingMethods
{
    public class FeatureMatchTypeManager
    {

        public static FeatureMatchTypes GetMatchType(MatchedFeature matchedFeature, StreetAddress inputAddress)
        {
            FeatureMatchTypes ret = FeatureMatchTypes.Unknown;

            try
            {
                // if the match is 100% then it's exact
                if (matchedFeature.MatchScore == 100)
                {
                    ret = FeatureMatchTypes.Exact;
                }
                else
                {
                    // if the street name matches, we have some type of relaxed or nearby
                    if (String.Compare(inputAddress.StreetName, matchedFeature.MatchedFeatureAddress.StreetName, true) == 0)
                    {
                        ret = FeatureMatchTypes.Relaxed;
                    }
                    else
                    {
                        // if the street name does not match, we have some type of soundex

                        // if all of the attributes match, it's an exact soundex
                        if (String.Compare(inputAddress.PreDirectional, matchedFeature.MatchedFeatureAddress.PreDirectional, true) == 0 &&
                            String.Compare(inputAddress.StreetName, matchedFeature.MatchedFeatureAddress.StreetName, true) == 0 &&
                            String.Compare(inputAddress.PostDirectional, matchedFeature.MatchedFeatureAddress.PostDirectional, true) == 0 &&
                            String.Compare(inputAddress.ZIP, matchedFeature.MatchedFeatureAddress.ZIP, true) == 0 &&
                            String.Compare(inputAddress.City, matchedFeature.MatchedFeatureAddress.City, true) == 0)
                        {
                            ret = FeatureMatchTypes.Soundex;
                        }
                        else
                        {
                            // if all of the attributes don't match, it's a relaxed soundex

                            ret = FeatureMatchTypes.Soundex | FeatureMatchTypes.Relaxed;
                        }
                    }


                    // set the nearby flag if appropriate - only street addresses can be nearby
                    if (matchedFeature.MatchedReferenceFeature.AddressComponent == AddressComponents.All)
                    {

                        // all nearby's must match the name, suffix, and city or zip
                        if (
                            String.Compare(inputAddress.StreetName, matchedFeature.MatchedFeatureAddress.StreetName, true) == 0 &&
                            String.Compare(inputAddress.Suffix, matchedFeature.MatchedFeatureAddress.Suffix, true) == 0 &&
                                (
                                String.Compare(inputAddress.City, matchedFeature.MatchedFeatureAddress.City, true) == 0 ||
                                String.Compare(inputAddress.ZIP, matchedFeature.MatchedFeatureAddress.ZIP, true) == 0
                                )
                            )
                        {

                            if (matchedFeature.MatchScoreResult.ParityResultType != FeatureMatchAddressParityResultType.CorrectParity || matchedFeature.MatchScoreResult.AddressRangeResultType != FeatureMatchAddressRangeResultType.WithinRange)
                            {
                                // dimes and nickles must be within range and have the correct parity, otherwise they are nearby
                                if (matchedFeature.MatchedReferenceFeature.ReferenceFeatureType == ReferenceFeatureType.Dime || matchedFeature.MatchedReferenceFeature.ReferenceFeatureType == ReferenceFeatureType.Nickle)
                                {
                                    if (matchedFeature.MatchScoreResult.ParityResultType != FeatureMatchAddressParityResultType.CorrectParity)
                                    {
                                        ret = ret | FeatureMatchTypes.Nearby;
                                    }
                                    if (matchedFeature.MatchScoreResult.AddressRangeResultType != FeatureMatchAddressRangeResultType.WithinRange)
                                    {
                                        ret = ret | FeatureMatchTypes.Nearby;
                                    }

                                }
                                // pennies must have the same number, otherwise they are nearby
                                else if (matchedFeature.MatchedReferenceFeature.ReferenceFeatureType == ReferenceFeatureType.Penny)
                                {
                                    if (String.Compare(inputAddress.Number, matchedFeature.MatchedFeatureAddress.Number, true) != 0)
                                    {
                                        ret = ret | FeatureMatchTypes.Nearby;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error in GetMatchScore: " + e.Message, e);
            }

            return ret;
        }
    }
}
