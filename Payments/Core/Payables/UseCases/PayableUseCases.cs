/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : PayablesUseCases                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for payables management.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial;

using Empiria.Payments.Payables.Adapters;
using Empiria.Payments.Payables.Data;

namespace Empiria.Payments.Payables.UseCases {

  /// <summary>Use cases for payables management.</summary>
  public class PayableUseCases : UseCase {

    #region Constructors and parsers

    protected PayableUseCases() {
      // no-op
    }

    static public PayableUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<PayableUseCases>();
    }

    #endregion Constructors and parsers

    #region Payable use cases

    public PayableDto CreatePayable(PayableFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      PayableType payableType = PayableType.Parse(fields.PayableTypeUID);

      IPayableEntity payableEntity = payableType.ParsePayableEntity(fields.PayableEntityUID);

      var payable = new Payable(payableType, payableEntity);

      payable.Update(fields);

      payable.Save();

      return PayableMapper.Map(payable);
    }


    public void DeletePayable(string payableUID) {
      Assertion.Require(payableUID, nameof(payableUID));

      var payable = Payable.Parse(payableUID);

      payable.Delete();

      payable.Save();
    }


    public PayableDto GetPayable(string payableUID) {
      var payable = Payable.Parse(payableUID);

      return PayableMapper.Map(payable);
    }


    public FixedList<PayableItemDto> GetPayableItems(string payableUID) {
      Assertion.Require(payableUID, nameof(payableUID));

      Payable payable = Payable.Parse(payableUID);

      FixedList<PayableItem> payableItems = payable.GetItems();

      return PayableItemMapper.Map(payableItems);
    }


    public FixedList<NamedEntityDto> GetPayableTypes() {

      FixedList<PayableType> payableTypes = PayableType.GetList();

      return payableTypes.Select(x => new NamedEntityDto(x.UID, x.DisplayName))
      .ToFixedList();
    }


    public FixedList<PayableDescriptor> SearchPayables(PayablesQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      string filter = query.MapToFilterString();
      string sortBy = query.MapToSortString();

      FixedList<Payable> payables = PayableData.GetPayables(filter, sortBy);

      return PayableMapper.MapToDescriptor(payables);
    }


    public PayableDto UpdatePayable(string payableUID, PayableFields fields) {
      Assertion.Require(payableUID, nameof(payableUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var payable = Payable.Parse(payableUID);

      payable.Update(fields);

      payable.Save();

      return PayableMapper.Map(payable);
    }

    #endregion Payable use cases

    #region PayableItem use cases

    public PayableItemDto AddPayableItem(string payableUID, PayableItemFields fields) {
      Assertion.Require(payableUID, nameof(payableUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var payable = Payable.Parse(payableUID);

      PayableItem payableItem = payable.CreateItem();

      payableItem.Update(fields);

      payable.AddItem(payableItem);

      payableItem.Save();

      return PayableItemMapper.Map(payableItem);
    }


    public void RemovePayableItem(string payableUID, string payableItemUID) {
      Assertion.Require(payableUID, nameof(payableUID));
      Assertion.Require(payableItemUID, nameof(payableItemUID));

      Payable payable = Payable.Parse(payableUID);

      PayableItem payableItem = payable.RemoveItem(payableItemUID);

      payableItem.Save();
    }


    public PayableItemDto UpdatePayableItem(string payableUID,
                                            string payableItemUID,
                                            PayableItemFields fields) {

      Assertion.Require(payableUID, nameof(payableUID));
      Assertion.Require(payableItemUID, nameof(payableItemUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Assertion.Require(payableUID = fields.PayableUID, "fields.PayableUID field mismatch.");
      Assertion.Require(payableItemUID = fields.UID, "fields.UID field mismatch.");

      Payable payable = Payable.Parse(payableUID);

      PayableItem payableItem = payable.UpdateItem(payableItemUID, fields);

      payableItem.Save();

      return PayableItemMapper.Map(payableItem);
    }

    #endregion PayableItem use cases

    #region Link use cases

    public PayableDto AddPayableLink(PayableLinkFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var payable = Payable.Parse(fields.PayableUID);
      var linkType = PayableLinkType.Parse(fields.LinkTypeUID);

      BaseObject linkedObject = linkType.ParseLinkedObject(fields.LinkedObjectUID);

      var link = new PayableLink(linkType, payable, linkedObject);

      link.Save();

      return PayableMapper.Map(payable);
    }


    public void RemovePayableLink(string payableUID, string payableLinkUID) {
      Assertion.Require(payableUID, nameof(payableUID));
      Assertion.Require(payableLinkUID, nameof(payableLinkUID));

      Payable payable = Payable.Parse(payableUID);

      PayableLink link = PayableLink.Parse(payableLinkUID);

      link.Save();
    }

    #endregion Link use cases

    #region PayableData

    public PayableDataDto GetPayableData(string payableUID) {
      var payable = Payable.Parse(payableUID);

      return PayableDataMapper.Map(payable);
    }
      

    #endregion PayableData

  }  // class PayableUseCases

}  // namespace Empiria.Payments.Payables.UseCases
