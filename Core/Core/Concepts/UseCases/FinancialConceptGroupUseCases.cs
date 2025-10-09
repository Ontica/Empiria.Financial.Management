/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases interactor                  *
*  Type     : FinancialConceptGroupUseCases                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for retrieve financial concepts in groups.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial.Concepts.Adapters;

namespace Empiria.Financial.Concepts.UseCases {

  /// <summary>Provides use cases for retrieve financial concepts in groups.</summary>
  public class FinancialConceptGroupUseCases : UseCase {

    #region Constructors and parsers

    protected FinancialConceptGroupUseCases() {
      // no-op
    }

    static public FinancialConceptGroupUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FinancialConceptGroupUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<FinancialConceptEntityDto> GetConceptsList(string groupNamedKey) {
      Assertion.Require(groupNamedKey, nameof(groupNamedKey));

      var group = FinancialConceptGroup.ParseWithNamedKey(groupNamedKey);

      FixedList<FinancialConcept> concepts = group.GetConcepts();

      return concepts.Select(x => new FinancialConceptEntityDto {
        UID = x.UID,
        Name = x.Name,
        FullName = x.FullName,
      })
      .ToFixedList();
    }


    public FixedList<FinancialConceptGroupDescriptor> GetGroups() {
      var groups = FinancialConceptGroup.GetList();

      return FinancialConceptGroupMapper.Map(groups);
    }


    public FinancialConceptGroupDto SearchConcepts(FinancialConceptsQuery query) {
      Assertion.Require(query, nameof(query));

      var group = FinancialConceptGroup.Parse(query.GroupUID);

      FixedList<FinancialConcept> concepts = query.Execute();

      return FinancialConceptGroupMapper.Map(group, concepts);
    }

    #endregion Use cases

  }  // class FinancialConceptGroupUseCases

}  // namespace Empiria.Financial.Concepts.UseCases
