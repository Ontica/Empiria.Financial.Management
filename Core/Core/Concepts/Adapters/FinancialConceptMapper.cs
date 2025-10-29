/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Interface adapters                      *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Mapper                                  *
*  Type     : FinancialConceptMapper                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for financial concepts.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Concepts.Adapters {

  /// <summary>Mapping methods for financial concepts.</summary>
  static public class FinancialConceptMapper {

    static internal FixedList<FinancialConceptDescriptor> MapToDescriptor(FixedList<FinancialConcept> list) {
      return list.Select((x) => MapToDescriptor(x))
                 .ToFixedList();
    }

    static internal FinancialConceptDescriptor MapToDescriptor(FinancialConcept concept) {
      return new FinancialConceptDescriptor {
        UID = concept.UID,
        Number = concept.ConceptNo,
        Name = concept.Name,
        FullName = concept.FullName,
        Level = concept.Level,
        IsLastLevel = concept.IsLastLevel,
        StatusName = concept.Status.GetName(),
        StartDate = concept.StartDate,
        EndDate = concept.EndDate,
      };
    }


    static internal FinancialConceptHolder MapToHolder(FinancialConcept concept) {
      return new FinancialConceptHolder {
        Concept = Map(concept),
        Integration = Map(concept.GetEntries()),
        Actions = MapActions(concept)
      };
    }

    #region Helpers

    static private FinancialConceptActions MapActions(FinancialConcept concept) {
      return new FinancialConceptActions {
        CanActivate = false,
        CanSuspend = false,
        CanUpdate = false,
      };
    }


    static private FinancialConceptDto Map(FinancialConcept concept) {
      return new FinancialConceptDto {
        UID = concept.UID,
        Number = concept.ConceptNo,
        Name = concept.Name,
        FullName = concept.FullName,
        Description = concept.Description,
        Group = concept.Group.MapToNamedEntity(),
        Level = concept.Level,
        IsLastLevel = concept.IsLastLevel,
        Status = concept.Status.MapToDto(),
        StartDate = concept.StartDate,
        EndDate = concept.EndDate
      };
    }


    static private FixedList<FinancialConceptEntryDescriptor> Map(FixedList<FinancialConceptEntry> entries) {
      return entries.Select(x => MapToDescriptor(x))
                    .ToFixedList();
    }


    static private FinancialConceptEntryDescriptor MapToDescriptor(FinancialConceptEntry entry) {
      return new FinancialConceptEntryDescriptor {
        UID = entry.UID,
        Name = entry.Name,
        TypeName = entry.TypeName,
        StartDate = entry.StartDate,
        EndDate = entry.EndDate,
        Operation = entry.Operation
      };
    }

    #endregion Helpers

  }  // class FinancialConceptMapper

}  // namespace Empiria.Financial.Concepts.Adapters
