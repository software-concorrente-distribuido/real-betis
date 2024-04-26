#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include <time.h>
#include <pthread.h>

#define NTHREADS 5

// Cada No é um processo
struct no {
    int id;
    int ticket;
    struct no* proximo;
    bool escolhendo;
};

struct lista {
    int quantidade;
    struct no* primeiro;
    struct no* ultimo;
};

typedef struct no No;
typedef struct lista Lista;

typedef struct thread_data {
    Lista *lista;
    int processoId;
    double tempo_gasto;

} thread_data;

No* criaNo(int id) {
    No* no = (No*)malloc(sizeof(No));
    no->id = id;
    no->ticket = 0;
    no->proximo = NULL;
    no->escolhendo = NULL;

    return no;
}

void liberaNo(No* no) {
    free(no);
}

Lista* criaLista() {
    Lista* lista = (Lista*)malloc(sizeof(Lista));
    lista->quantidade = 0;
    lista->primeiro = NULL;
    lista->ultimo = NULL;

    return lista;
}

void liberaLista(Lista* lista) {
    No* noAux = lista->primeiro;

    while (noAux != NULL) {
        No* noAux2 = noAux->proximo;
        liberaNo(noAux);

        noAux = noAux2;
    }

    free(lista);
}

void addNo(Lista* lista, No* no) {
    if (lista->quantidade == 0) {
        lista->primeiro = no;
        lista->ultimo = no;
        lista->quantidade = 1;

        return;
    }

    No* penultimo = lista->ultimo;
    penultimo->proximo = no;
    lista->ultimo = no;
    lista->quantidade++;
}
/*
true - função removeu com sucesso
false - erro (tentando acessar no inexistente)
*/
bool removeNo(Lista* lista, int indice) {
    if (lista->quantidade < indice + 1) {
        return false;
    }
    //Remover primeiro e último
    if (indice == 0 && lista->quantidade == 1) {
        No* removido = lista->primeiro;
        lista->primeiro = NULL;
        lista->ultimo = NULL;
        lista->quantidade--;
        liberaNo(removido);
        return true;
    }
    //Remover o primeiro
    if (indice == 0) {
        No* removido = lista->primeiro;
        lista->primeiro = removido->proximo;
        lista->quantidade--;
        liberaNo(removido);
        return true;
    }

    No* anterior = lista->primeiro;
    for (int aux = 1; aux < indice; aux++) {
        anterior = anterior->proximo;
    }
    //Remover último
    if (lista->quantidade == indice + 1) {
        No* removido = anterior->proximo;
        anterior->proximo = NULL;
        lista->ultimo = anterior;
        lista->quantidade--;
        liberaNo(removido);
        return true;
    }
    No* removido = anterior->proximo;
    anterior->proximo = removido->proximo;
    lista->quantidade--;
    liberaNo(removido);
    return true;
}

// Retorna NULL caso não seja encontrado um No a partir do indice fornecido
No* getNo(Lista* lista, int indice) {
        if (lista->quantidade < indice + 1) {
        return NULL;
    }

    No* desejado = lista->primeiro;
    for (int aux = 0; aux < indice; aux++) {
        desejado = desejado->proximo;
    }

    return desejado;
}

int maiorTicket(Lista *lista) {
    int maior = 0;
    for (int indice = 0; indice < NTHREADS; indice++) {
        int ticket = getNo(lista, indice)->ticket;
        maior =  ticket > maior ? ticket : maior;
    }

    return maior;
}

void iniciaLista(Lista *lista) {
    for (int indice = 0; indice < NTHREADS; indice++) {
        addNo(lista, criaNo(indice));
    }
}

void persisteString(char *string, char *nomeArquivo) {
    FILE *file;

    file = fopen(nomeArquivo, "a+");
    fprintf(file, "%s\n", string);
    fclose(file);
}

void antes_de_entrar_na_regiao_critica(Lista *lista, int processoId) {
    No *processo = getNo(lista, processoId);
    processo->escolhendo = true;
    // Pode acontecer de dois processos terem o mesmo ticket, o outro criterio para desempate é o id
    processo->ticket = maiorTicket(lista) + 1;
    processo->escolhendo = false;

    for (int indice = 0; indice < NTHREADS; indice++) {
        No *processoAux = getNo(lista, indice);
        
        while(processoAux->escolhendo) {
            // Não faz nada | Espera ocupada
        }
        while((processoAux->ticket != 0) && ((processoAux->ticket < processo->ticket) || ((processoAux->ticket == processo->ticket) && (indice < processoId)))) {
            // Não faz nada | Espera ocupada
        }
    }
}

void depois_de_sair_da_regiao_critica(Lista *lista, int processoId) {
    No *processo = getNo(lista, processoId);
    processo->ticket = 0;
}

//--------------------------------------------------------
void regiao_critica(int processoId) {
    char *nomeArquivo = "bonitim.txt";
    char string[20];
    sprintf(string, "Processo %d\n", processoId);
    for (int indice = 0; indice < 5000; indice++) {
        persisteString(string, nomeArquivo);
    }
}

void processamento_sem_regiao_critica(int processoId) {
    char *nomeArquivo = "baguncadim.txt";
    char string[20];
    sprintf(string, "Processo %d\n", processoId);
    for (int indice = 0; indice < 10000; indice++) {
        persisteString(string, nomeArquivo);
    }
}

void *thread(void *arg) {
    thread_data *tdata = (thread_data *)arg;

    clock_t inicio_processo0, fim_processo0;

    inicio_processo0 = clock();
    antes_de_entrar_na_regiao_critica(tdata->lista, tdata->processoId);
    regiao_critica(tdata->processoId);
    depois_de_sair_da_regiao_critica(tdata->lista, tdata->processoId);
    processamento_sem_regiao_critica(tdata->processoId);
    fim_processo0 = clock();

    tdata->tempo_gasto = ((double) fim_processo0 - inicio_processo0) / CLOCKS_PER_SEC;
    pthread_exit(NULL);
}
//--------------------------------------------------------



int main() {
    Lista *lista = criaLista();
    iniciaLista(lista);

    pthread_t threads[NTHREADS];
    thread_data tdata[NTHREADS];
    for (int indice = 0; indice < NTHREADS; indice++) {
        tdata[indice].lista = lista;
        tdata[indice].processoId = indice;
    }

    clock_t inicio, fim;
    inicio = clock();
    for (int indice = 0; indice < NTHREADS; indice++) {
        printf("Thread %d foi criada\n", indice);
        printf("--------------\n");
        pthread_create(&threads[indice], NULL, thread, (void *)&tdata[indice]);
    }

    for (int indice = 0; indice < NTHREADS; indice++) {
        pthread_join(threads[indice], NULL);
    }
    fim = clock();
    double tempo_programa = ((double) fim - inicio) / CLOCKS_PER_SEC;

    for (int indice = 0; indice < NTHREADS; indice++) {
        printf("Tempo gasto pela Thread %d: %.2f ms\n", indice, tdata[indice].tempo_gasto * 1000);
    }
    printf("Tempo do programa: %.2f ms\n", tempo_programa * 1000);

    liberaLista(lista);
    return 0;
}

