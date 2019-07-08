CREATE PROCEDURE [ray].[InvSp_ValidSoh] 
(@FiscalYear as int,@StoreNo as varchar(6),@PartNo as varchar(50),@Qty as money,@DocOPerator as bit,
@P_DocDate as varchar(8),@Pre_Docdate as varchar(8),@Pre_Qty as money,@PreOperator as bit,@DocType as int,@Valid as bit output
) 
with Encryption
AS


Declare @TmpQty as money
Declare @Tmp_Qty as money
Declare @Tmpre_Qty as money
declare @IsResQtySale as bit  --موجودي رزرو  لحاظ شود
declare @QtyRes as money --موجودي رزرو فروش 
declare @QtyResNext as money --موجودي رزرو فعلا از آن استفاده نمي شود
declare @EndDate as varchar(8)

set @IsResQtySale=0
set @QtyRes=0
set  @Valid=1
set @EndDate=''
--پيدا كردن موجودي رزرو فروش براي سند بجز حواله(فروش) براساس تنظيمات
if @DocType<>40
    begin
			select @IsResQtySale=isnull(IsResQtySale,0)  from ray.store with(nolock) where storeno=@StoreNo
			if @IsResQtySale=1 --اگر قرار است لحاظ شود
			   begin
                          SELECT  @EndDate=EndFiscalYear FROM ray.InvPrd with(nolock) WHERE FiscalYear =@FiscalYear
                          --موجودي رزرو فروش تا آخر
                         --حتما بايد در تنظيمات سيستم فروش يا كنترل موجودي پيش فاكتور باشد يا كنترل موجودي حواله
                          exec ray.SaleInvRes  @EndDate,@FiscalYear,@StoreNo,@PartNo,@QtyRes output,@QtyResNext output
                         
			   end
   end
  
-----------------------------------------------------------------------


         If (@Pre_Qty = 0 And @Pre_Docdate='') 
             begin
	            If @DocOPerator=0 
	                 set   @Qty = -@Qty
	            If @Qty < 0  
	               begin              
	                   select @TmpQty=qty-@QtyRes from ray.invarcsoh i with(nolock) ,(select fiscalyear,storeno,partno,max(docdate) as dd from ray.invarcsoh with(nolock) where fiscalyear=@FiscalYear
	                               and storeno=@Storeno  and docdate <= @P_DocDate  and partno=@PartNo group by fiscalyear,storeno,partno) as tt
	                                where i.fiscalyear=tt.fiscalyear and i.storeno=tt.storeno and i.partno=tt.partno and i.docdate=tt.dd
	                   if @@RowCount =0 
	                         set   @Valid = 0
	                   Else
	                         If @Qty + @TmpQty < 0 
	                              set @Valid = 0
	                   
	              end                      
	              select * from ray.invarcsoh with(nolock) where fiscalyear=@FiscalYear  and storeno=@Storeno  and docdate >= @P_DocDate 
	                        and partno=@PartNo  and ( qty-@QtyRes + @Qty  <0 )
          End
         ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         Else
           begin
	          If @P_DocDate > @Pre_Docdate 
	             begin                 
	             
	                   if (@PreOperator=1 and @Pre_Qty<0) or (@PreOperator=0 and -@Pre_Qty<0) 
		               begin 
	                              select @TmpQty=qty-@QtyRes from ray.invarcsoh i with(nolock) ,(select fiscalyear,storeno,partno,max(docdate) as dd from ray.invarcsoh with(nolock) where fiscalyear=@FiscalYear
		                                 and storeno=@Storeno  and docdate <= @Pre_Docdate  and partno=@PartNo group by fiscalyear,storeno,partno) as tt
		                                  where i.fiscalyear=tt.fiscalyear and i.storeno=tt.storeno and i.partno=tt.partno  and i.docdate=tt.dd
		                     if @@RowCount =0 
		                         set @Valid=0    
		                     Else
		                             if  (@PreOperator=1 and   @Pre_Qty +@TmpQty <0 )or (@PreOperator=0 and -@Pre_Qty +@TmpQty <0)
	                                            set   @Valid=0    
	                              End
	                 
	                     if @DocOPerator=1
	                           set @Tmp_Qty=@Qty
	                     else
	                           set @Tmp_Qty=-@Qty
	                     if @PreOperator=1
	                            set @Tmpre_Qty=@Pre_Qty
	                     else
	                            set @Tmpre_Qty=-@Pre_Qty
	                    
	                      if (@Tmp_Qty+@Tmpre_Qty)<0 
	                         begin 
									select @TmpQty=qty-@QtyRes from ray.invarcsoh i with(nolock),(select fiscalyear,storeno,partno,max(docdate) as dd from ray.invarcsoh with(nolock) where fiscalyear=@FiscalYear
									and storeno=@Storeno  and docdate <= @P_DocDate  and partno=@PartNo group by fiscalyear,storeno,partno) as tt
									where i.fiscalyear=tt.fiscalyear and i.storeno=tt.storeno and i.partno=tt.partno
									and i.docdate=tt.dd
	                                if @@RowCount =0 
		                                  set  @Valid=0    
		                     Else
	                                If (@Tmp_Qty+@Tmpre_Qty + @TmpQty) < 0 
	                                       set   @Valid=0    
	                         End 
	                    
	                  select * from ray.invarcsoh with(nolock) where fiscalyear=@FiscalYear  and storeno=@Storeno  and partno=@PartNo  and qty < case 
	                        when docdate >= @Pre_Docdate  and docdate < @P_DocDate  then -@Tmpre_Qty+@QtyRes when docdate >= @P_DocDate  then -@Tmp_Qty-@Tmpre_Qty+@QtyRes end 
 	            End       
                    -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    else
                       begin
	                 If @P_DocDate < @Pre_Docdate 
                                begin
                                     if (@DocOPerator=1 and @Qty<0) or (@DocOPerator=0 and -@Qty<0) 
                                        begin
                                                  select @TmpQty=qty-@QtyRes from Ray.invarcsoh i with(nolock),(select fiscalyear,storeno,partno,max(docdate) as dd from Ray.invarcsoh with(nolock) where fiscalyear=@FiscalYear 
                                                        and storeno=@Storeno  and docdate <= @P_DocDate   and partno=@PartNo group by fiscalyear,storeno,partno) as tt
                                                        where i.fiscalyear=tt.fiscalyear and i.storeno=tt.storeno and i.partno=tt.partno and i.docdate=tt.dd
                   
                                                 if @@RowCount =0 
		                                             set  @Valid=0    
            	 	                      Else
                                                     begin                    
                                                        if (@DocOPerator=1)
															   begin
																  If @Qty + @TmpQty < 0   set  @Valid=0    
															   end
                                                         else
                                                                If -@Qty + @TmpQty < 0   set  @Valid=0    
                                                         end
                                         end  
                                          if @DocOPerator=1
	                                       set @Tmp_Qty=@Qty
	                                else
			                    set @Tmp_Qty=-@Qty
                                          if @PreOperator=1
			                     set @Tmpre_Qty=@Pre_Qty
			             else
			                     set @Tmpre_Qty=-@Pre_Qty
                                     
                                             if (@Tmp_Qty+@Tmpre_Qty)<0 
                                                begin 
														select qty-@QtyRes from ray.invarcsoh i with(nolock),(select fiscalyear,storeno,partno,max(docdate) as dd from ray.invarcsoh with(nolock) where fiscalyear=@FiscalYear 
														and storeno=@Storeno  and docdate <= @Pre_Docdate  and partno=@PartNo group by fiscalyear,storeno,partno) as tt 
														where i.fiscalyear=tt.fiscalyear and i.storeno=tt.storeno and i.partno=tt.partno  and i.docdate=tt.dd 
                                                        if @@RowCount =0 
	                                                           set  @Valid=0    
            	 	                          Else
                                                         If @Qty + @Pre_Qty+ @TmpQty < 0 
                                                                set  @Valid=0    
                                              End 
									  		select * from Ray.invarcsoh with(nolock) where fiscalyear=@FiscalYear 
											and storeno=@Storeno  and partno=@PartNo  and qty < case 
											when docdate >= @P_DocDate  and docdate < @Pre_Docdate  then  -@Tmp_Qty+@QtyRes
											when docdate >= @Pre_Docdate  then (-@Tmp_Qty-@Tmpre_Qty+@QtyRes ) end 

                                       end
                                 ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                                  Else
	                          begin
									 if @P_DocDate = @Pre_Docdate		              
													begin
														   If  @DocOPerator=0 
														   set @Qty = -@Qty
														   If  @PreOperator=0  
														   set @Pre_Qty = -@Pre_Qty
														   set  @Qty = @Qty + @Pre_Qty
		                     
															If @Qty < 0 
															  begin
																		select @TmpQty=qty-@QtyRes from ray.invarcsoh i with(nolock),(select fiscalyear,storeno,partno,max(docdate) as dd from ray.invarcsoh with(nolock) where fiscalyear=@FiscalYear
																		and storeno=@Storeno  and docdate <= @P_DocDate  and partno=@PartNo group by fiscalyear,storeno,partno) as tt  where i.fiscalyear=tt.fiscalyear and i.storeno=tt.storeno and i.partno=tt.partno
																		and i.docdate=tt.dd

										 							   if @@RowCount =0 
																					set  @Valid=0    
																		Else
																					If @Qty + @TmpQty < 0 
														     							set  @Valid=0    
																		End          
			                											select * from ray.invarcsoh with(nolock) where fiscalyear=@FiscalYear  and storeno=@Storeno  and docdate >= @P_DocDate 
																		 and partno=@PartNo  and ( qty-@QtyRes + @Qty  <0 )
																 End                
													  End
								 End
         End

         if @@RowCount <>0 
	      set  @Valid=0