#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include <time.h>
#include <pthread.h>

#define NTHREADS 2

// Variáveis globais / compartilhadas
// Indica de qual processo é a vez. Do processo 0 ou do processo 1.
int vez;
// Indica qual processo está pronto / iminente para entrar na região crítica
bool quem_esta_pronto[2];

void inicia_variaveis() {
    quem_esta_pronto[0] = false;
    quem_esta_pronto[1] = false;
}

typedef struct thread_data {
    int processo;
    double tempo_gasto;

} thread_data;

void persisteString(char *string, char *nomeArquivo) {
    FILE *file;

    file = fopen(nomeArquivo, "a+");
    fprintf(file, "%s\n", string);
    fclose(file);
}

//------------------------------------------------------
void antes_de_entrar_na_regiao_critica(int processo) {
    quem_esta_pronto[processo] = true;

    int outro_processo = (processo == 0 ? 1 : 0);
    // Humble guy! Da a vez ao outro processo.
    vez = outro_processo;
    /*
    Enquanto a vez for do outro processo e o outro processo estiver pronto = Outro processo acessando a região crítica.
    Ocasionando em uma espera ocupada deste processo processo.
    */
    while (vez == outro_processo && quem_esta_pronto[outro_processo]) {
        // Não está fazendo nada | Espera ocupada.
    }
}

void depois_de_sair_da_regiao_critica(int processo) {
    quem_esta_pronto[processo] = false;
}
//--------------------------------------------------------

//--------------------------------------------------------
void regiao_critica_processo_0() {
    char *nomeArquivo = "bonitim.txt";
    for (int indice = 0; indice < 50000; indice++) {
        persisteString("Processo 0\n", nomeArquivo);
    }
}

void processamento_sem_regiao_critica_processo_0() {
    char *nomeArquivo = "baguncadim.txt";
    for (int indice = 0; indice < 100000; indice++) {
        persisteString("Processo 0\n", nomeArquivo);
    }
}

void *processo_0(void *arg) {
    thread_data *tdata = (thread_data *)arg;

    clock_t inicio_processo0, fim_processo0;

    inicio_processo0 = clock();
    antes_de_entrar_na_regiao_critica(tdata->processo);
    regiao_critica_processo_0();
    depois_de_sair_da_regiao_critica(tdata->processo);
    processamento_sem_regiao_critica_processo_0();
    fim_processo0 = clock();

    tdata->tempo_gasto = ((double) fim_processo0 - inicio_processo0) / CLOCKS_PER_SEC;
    pthread_exit(NULL);
}
//--------------------------------------------------------

//--------------------------------------------------------
void regiao_critica_processo_1() {
    char *nomeArquivo = "bonitim.txt";
    for (int indice = 0; indice < 50000; indice++) {
        persisteString("Processo 1\n", nomeArquivo);
    }
}

void processamento_sem_regiao_critica_processo_1() {
    char *nomeArquivo = "baguncadim.txt";
    for (int indice = 0; indice < 100000; indice++) {
        persisteString("Processo 1\n", nomeArquivo);
    }
}


void *processo_1(void *arg) {
    thread_data *tdata = (thread_data *)arg;

    clock_t inicio_processo1, fim_processo1;

    inicio_processo1 = clock();
    antes_de_entrar_na_regiao_critica(tdata->processo);
    regiao_critica_processo_1();
    depois_de_sair_da_regiao_critica(tdata->processo);
    processamento_sem_regiao_critica_processo_1();
    fim_processo1 = clock();

    tdata->tempo_gasto = ((double) fim_processo1 - inicio_processo1) / CLOCKS_PER_SEC;
    pthread_exit(NULL);
}
//--------------------------------------------------------


int main() {
    // Alocar memória e iniciar os dois processos como NÂO prontos
    inicia_variaveis();

    pthread_t threads[NTHREADS];
    thread_data tdata[NTHREADS];
    tdata[0].processo = 0;
    tdata[1].processo = 1;

    clock_t inicio, fim;
    inicio = clock();
    printf("Thread 0 foi criada\n");
    printf("--------------\n");
    pthread_create(&threads[0], NULL, processo_0, (void *)&tdata[0]);

    printf("Thread 1 foi criada\n");
    printf("--------------\n");
    pthread_create(&threads[1], NULL, processo_1, (void *)&tdata[1]);

    pthread_join(threads[0], NULL);
    pthread_join(threads[1], NULL);
    fim = clock();
    double tempo_programa = ((double) fim - inicio) / CLOCKS_PER_SEC;

    printf("Tempo do processo 0: %.2f ms\n", tdata[0].tempo_gasto * 1000);
    printf("Tempo do processo 1: %.2f ms\n", tdata[1].tempo_gasto * 1000);
    printf("Tempo do programa: %.2f ms\n", tempo_programa * 1000);

    return 0;
}