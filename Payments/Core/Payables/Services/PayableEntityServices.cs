/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Services Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service interactor class                *
*  Type     : PayableEntityServices                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Services used to retrive and communicate with payable entities.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Payments.Payables.Adapters;

namespace Empiria.Payments.Payables.Services {

  /// <summary>Services used to retrive and communicate with payable entities.</summary>
  public class PayableEntityServices : Service {

    #region Constructors and parsers

    protected PayableEntityServices() {
      // no-op
    }

    static public PayableEntityServices ServiceInteractor() {
      return CreateInstance<PayableEntityServices>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<PayableEntityDto> SearchPayableEntities(PayableEntitiesQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      // ToDo: Change this fixed code to search for any payable types

      var payableType = PayableType.Parse(query.PayableTypeUID);

      var milestones = BaseObject.GetFullList<Contracts.ContractMilestone>()
                                 .ToFixedList()
                                 .FindAll(x => x.Status != StateEnums.EntityStatus.Deleted);

      return PayableEntityMapper.Map(milestones);
    }

    #endregion Use cases

  }  // class PayableEntityServices

}  // namespace Empiria.Payments.Payables.UseCases
