SELECT 
    o.id AS operation_id,
    o.quantity,
    o.price,
    o.operation_type,
    o.brokerage_fee,
    o.date_time
FROM Operations o
WHERE o.user_id = ? -- Valor do Id do usuário
  AND o.asset_id = ? -- Valor do Id do ativo
  AND o.date_time >= NOW() - INTERVAL 30 DAY
ORDER BY o.date_time DESC;