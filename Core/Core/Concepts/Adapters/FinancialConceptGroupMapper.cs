/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Interface adapters                      *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Mapper                                  *
*  Type     : FinancialConceptGroupMapper                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for financial concepts groups.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Concepts.Adapters {

  /// <summary>Mapping methods for financial concepts groups.</summary>
  static public class FinancialConceptGroupMapper {

    static internal FixedList<FinancialConceptGroupDescriptor> Map(FixedList<FinancialConceptGroup> groups) {
      return groups.Select((x) => MapToDescriptor(x))
                   .ToFixedList();
    }


    static internal FinancialConceptGroupDto Map(FinancialConceptGroup group) {
      return new FinancialConceptGroupDto {
        UID = group.UID,
        Name = group.Name,
        NamedKey = group.NamedKey,
        Concepts = FinancialConceptMapper.MapToDescriptor(group.GetConcepts())
      };
    }


    static internal FinancialConceptGroupDto Map(FinancialConceptGroup group,
                                                 FixedList<FinancialConcept> concepts) {
      return new FinancialConceptGroupDto {
        UID = group.UID,
        Name = group.Name,
        Concepts = FinancialConceptMapper.MapToDescriptor(concepts)
      };
    }

    #region Helpers

    static private FinancialConceptGroupDescriptor MapToDescriptor(FinancialConceptGroup x) {
      return new FinancialConceptGroupDescriptor {
        UID = x.UID,
        Name = x.Name,
        NamedKey = x.NamedKey,
        IsReadOnly = true
      };
    }

    #endregion Helpers

  }  // class FinancialConceptGroupMapper

}  // namespace Empiria.Financial.Concepts.Adapters
