function ErrorCode = Verify( parameters,market,agents )
%VERIFY 此处显示有关此函数的摘要
%   此处显示详细说明
ErrorCode=zeros(length(market)-1,1);
for i=1:length(market)-1
    ErrorCode(i)=verifyTurn(parameters,market(market(:,1)==i-1|market(:,1)==i,:),agents(agents(:,1)==i-1|agents(:,1)==i,:),i);
end

    function ErrorCode=verifyTurn(parameters,market,agents,i)
        oldStocks=agents(agents(:,1)==i-1,4);
        tradeStocks=agents(agents(:,1)==i,7);
        oldCash=agents(agents(:,1)==i-1,3);
        dividend=agents(agents(:,1)==i,6);
        % 统计交易数量
        if market(2,4)~=sum(tradeStocks)||market(2,2)~=round(market(1,2)*exp(parameters.Lambda*sum(tradeStocks)/(length(tradeStocks)+1)),2)
            ErrorCode=1;
            return;
        end
        stockTmp=oldStocks+tradeStocks;
        cashTmp=oldCash-tradeStocks*market(1,2)-abs(tradeStocks)*parameters.TradeFee+dividend.*stockTmp;
        cashOutIndex=find(cashTmp<0);
        while ~isempty(cashOutIndex)
            cashTmp(cashOutIndex)=cashTmp(cashOutIndex)+market(2,2);
            stockTmp(cashOutIndex)=stockTmp(cashOutIndex)-1;
            cashOutIndex=find(cashTmp<0);
        end
        if any(stockTmp~=agents(agents(:,1)==i,4))||any(cashTmp~=agents(agents(:,1)==i,3))
            ErrorCode=2;
            return;
        end
        ErrorCode=0;
    end

end

