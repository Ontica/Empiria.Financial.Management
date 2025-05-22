/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : CashFlowProjectionUseCases                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrieve cash flow projections.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Accounts.Adapters;

using Empiria.CashFlow.Projections.Data;
using Empiria.CashFlow.Projections.Adapters;

namespace Empiria.CashFlow.Projections.UseCases {

  /// <summary>Use cases used to retrieve cash flow projections.</summary>
  public class CashFlowProjectionUseCases : UseCase {

    #region Constructors and parsers

    protected CashFlowProjectionUseCases() {
      // no-op
    }

    static public CashFlowProjectionUseCases UseCaseInteractor() {
      return CreateInstance<CashFlowProjectionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<NamedEntityDto> GetOperationSources() {
      return OperationSource.GetList()
                            .FindAll(x => x.Id == 11 || x.Id == 13)
                            .MapToNamedEntityList();
    }


    public CashFlowProjectionHolderDto GetProjection(string projectionUID) {
      Assertion.Require(projectionUID, nameof(projectionUID));

      var projection = CashFlowProjection.Parse(projectionUID);

      return CashFlowProjectionMapper.Map(projection);
    }


    public FixedList<CashFlowProjectionDescriptorDto> SearchProjections(CashFlowProjectionsQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();

      string sort = query.MapToSortString();

      FixedList<CashFlowProjection> projections = CashFlowProjectionDataService.SearchProjections(filter, sort);

      return CashFlowProjectionMapper.MapToDescriptor(projections);
    }


    public FixedList<NamedEntityDto> SearchProjectionsParties(TransactionPartiesQuery query) {
      var persons = BaseObject.GetList<Person>();

      return persons.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class CashFlowProjectionUseCases

}  // namespace Empiria.CashFlow.Projections.UseCases
