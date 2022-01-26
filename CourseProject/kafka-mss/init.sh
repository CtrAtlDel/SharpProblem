# Используйте это для создания топиков. В качестве альтернативы можно открыть kafdrop и создать топики из него
docker run --rm --entrypoint="" bitnami/kafka:2.8.1 /opt/bitnami/kafka/bin/kafka-topics.sh \
  --create --bootstrap-server 192.168.1.195:29092 --topic mss.worker.rq --partitions 3 --replication-factor 1 
docker run --rm --entrypoint="" bitnami/kafka:2.8.1 /opt/bitnami/kafka/bin/kafka-topics.sh \
  --create --bootstrap-server 192.168.1.195:29092 --topic mss.worker.rs --partitions 1 --replication-factor 1 