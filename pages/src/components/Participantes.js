import '../App'

const participantes = ["Daniel Nogueira", "Felipe Moreira", "Lucas Bernardes", "João Pedro Silva", "Matheus Geraldino"];

function Participantes() {
    return (
        <div className="Participantes">
            {
                participantes.map((nome, index) => (
                    <p key={index}>{nome}</p>
                    )
                )
            }
        </div>
    );
}

export default Participantes;