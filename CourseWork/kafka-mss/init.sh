# Используйте это для создания топиков. В качестве альтернативы можно открыть kafdrop и создать топики из него
docker run --rm --entrypoint="" bitnami/kafka:2.8.1 /opt/bitnami/kafka/bin/kafka-topics.sh \
  --create --bootstrap-server 172.16.185.183:29092 --topic mss.worker.rq --partitions 10 --replication-factor 1 
docker run --rm --entrypoint="" bitnami/kafka:2.8.1 /opt/bitnami/kafka/bin/kafka-topics.sh \
  --create --bootstrap-server 172.16.185.183:29092 --topic mss.worker.rs --partitions 1 --replication-factor 1 