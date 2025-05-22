/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.Cashflow.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : CashflowProjectionUseCases                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrieve cashflow projections.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Accounts.Adapters;

using Empiria.Cashflow.Projections.Data;
using Empiria.Cashflow.Projections.Adapters;

namespace Empiria.Cashflow.Projections.UseCases {

  /// <summary>Use cases used to retrieve cashflow projections.</summary>
  public class CashflowProjectionUseCases : UseCase {

    #region Constructors and parsers

    protected CashflowProjectionUseCases() {
      // no-op
    }

    static public CashflowProjectionUseCases UseCaseInteractor() {
      return CreateInstance<CashflowProjectionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public CashflowProjectionHolderDto GetProjection(string projectionUID) {
      Assertion.Require(projectionUID, nameof(projectionUID));

      var projection = CashflowProjection.Parse(projectionUID);

      return CashflowProjectionMapper.Map(projection);
    }


    public FixedList<CashflowProjectionDescriptorDto> SearchProjections(CashflowProjectionsQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();

      string sort = query.MapToSortString();

      FixedList<CashflowProjection> projections = CashflowProjectionDataService.SearchProjections(filter, sort);

      return CashflowProjectionMapper.MapToDescriptor(projections);
    }


    public FixedList<NamedEntityDto> SearchProjectionsParties(TransactionPartiesQuery query) {
      var persons = BaseObject.GetList<Person>();

      return persons.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class CashflowProjectionUseCases

}  // namespace Empiria.Cashflow.Projections.UseCases
