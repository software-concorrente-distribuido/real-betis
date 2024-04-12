#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <semaphore.h>

#define BUFFER_SIZE 5
#define MAX_ITEMS 20

int buffer[BUFFER_SIZE];

int in = 0;
int out = 0;
int produced_count = 0;
int consumed_count = 0;

sem_t mutex;
sem_t full;
sem_t empty;

void* producer(void* arg) {
   int item = 1;

   while (produced_count < MAX_ITEMS) {
      //DIMINUI A QUANTIDADE NO BUFFER DE ESPACOS VAZIOS DESDE QUE SEJA MAIOR DO QUE 0, SE NÃO ESTIVER AGUARDA ATÉ QUE O CONSUMIDOR LIBERE
      sem_wait(&empty);

      //ENTRA NO LOCK
      sem_wait(&mutex);

      //COLOCA O ITEM NO BUFFER
      buffer[in] = item;
      printf("Produced: %d \n", item);
      item++;
      in = (in + 1) % BUFFER_SIZE;

      produced_count++;

      //SAI DO LOCK
      sem_post(&mutex);

      //AUMENTA A QUANTIDADE DE ESPACOS CHEIOS
      sem_post(&full);
   }

   pthread_exit(NULL);
}

void* consumer(void* arg) {
   while (consumed_count < MAX_ITEMS) {
      //DIMINUI A QUANTIDADE DE ESPACOS CHEIOS DESDE QUE SEJA MAIOR QUE 0
      sem_wait(&full);

      //ENTRA NO LOCK
      sem_wait(&mutex);

      //RETIRA O ITEM DO BUFFER
      int item = buffer[out];
      printf("Consumed: %d \n", item);
      out = (out + 1) % BUFFER_SIZE;

      consumed_count++;

      //SAI DO LOCK
      sem_post(&mutex);

      //AUMENTA A QUANTIDADE DE ESPACOS VAZIOS
      sem_post(&empty);
   }

   pthread_exit(NULL);
}

int main() {
   //CRIANDO AS THREADS DO PRODUTOR E CONSUMIDOR
   pthread_t producerThread, consumerThread;

   //INSTANCIANDO OS SEMAFOROS
   //VERIFICA A QUANTIDADE DE ESPACOS VAZIOS NO BUFFER E SETA O VALOR INICIAL QUE É O TAMANHO DO PROPRIO BUFFER
   //ESSE TAMANHO É LIMITANTE PARA A QUANTIDADE DE ITENS QUE PODEM SER PRODUZINDOS ANTES QUE O CONSUMIDOR CONSUMA
   sem_init(&empty, 0, BUFFER_SIZE);

   //VERIFICA A QUANTIDADE DE ESPACOS CHEIOS NO BUFFER
   sem_init(&full, 0, 0);

   //SEMAFORO PARA INDICACAO DA ZONA CRITICA
   //GARANTINDO QUE SÓ UMA THREAD ENTRE POR VEZ
   sem_init(&mutex, 0, 1);

   //CRIANDO AS THREADS 
   pthread_create(&producerThread, NULL, producer, NULL);
   pthread_create(&consumerThread, NULL, consumer, NULL);

   //GARANTE AS EXECUCOES DAS THREADS 
   pthread_join(producerThread, NULL);
   pthread_join(consumerThread, NULL);

   //EXCLUSÃO DOS SEMAFOROS
   sem_destroy(&mutex);
   sem_destroy(&full);
   sem_destroy(&empty);

   return 0;
}