exec ray.InvSp_UpdateInvMstrOneDoc 1396(fiscalyear),'10'(storeno),40(doctype),2(docno)




exec ray.InvSp_DelInvSerial 1396(fiscalyear),'10'(storeno)




exec ray.InvSp_UpdateInvSerial 1396(fiscalyear),'10'(storeno)




exec ray.InvSp_UpdateInvArcSerialSoh 1396(fiscalyear),'10'(storeno)
,40(doctype),2(docno)

exec ray.InvSp_UpdateInvArcSohOneDoc 1396(fiscalyear),'10'(storeno),40(doctype),2(docno),'13960414'(updatedate)
