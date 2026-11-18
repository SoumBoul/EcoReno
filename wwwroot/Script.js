/*
Le JavaScript est séparé du HTML pour faciliter la lecture
et la maintenance. Ce fichier gère le formulaire et l'affichage
des demandes de la page.
*/

    const list = document.getElementById('demande-list');
    const form = document.getElementById('demande-form');

    const errorSlot = document.getElementById('form-error');

    function showError(message) {
        errorSlot.textContent = message;
        errorSlot.className = 'form-error';
        errorSlot.hidden = false;
      }

    function showSuccess(message) {
        errorSlot.textContent = message;
    errorSlot.className = 'form-success';
    errorSlot.hidden = false;
      }

    function clearMessage() {
        errorSlot.textContent = '';
    errorSlot.hidden = true;
      }

      // Transforme la réponse de validation ASP.NET Core (RFC 9110) en message lisible :
      // {"errors": {"Impact": ["L'impact estimé est obligatoire"] } }
    function describeValidationErrors(problem) {
        if (!problem || !problem.errors) {
          return null;
        }
    return Object.entries(problem.errors)
          .map(([field, messages]) => `${field} : ${[].concat(messages).join(' ')}`)
    .join(' | ');
      }

    async function loadDemandes() {
        try {
          const response = await fetch('/api/demandes');
    if (!response.ok) {
        list.innerHTML = '';
    const message = document.createElement('p');
    message.className = 'muted';
    message.textContent = 'Impossible de charger les demandes.';
    list.appendChild(message);
    return;
          }

    const demandes = await response.json();
    list.innerHTML = '';

    if (!Array.isArray(demandes) || !demandes.length) {
            const empty = document.createElement('p');
    empty.className = 'muted';
    empty.textContent = 'Aucune demande pour le moment.';
    list.appendChild(empty);
    return;
          }

          demandes.forEach((demande) => {
            const item = document.createElement('article');
    item.className = 'demande-item';

    // Construction du DOM via textContent : neutralise toute injection de balises
    // (XSS stocké) provenant des champs saisis par l'utilisateur.
    const title = document.createElement('h3');
    title.textContent = demande.name ?? '';
    item.appendChild(title);

    const rows = [
    {text: `Client : ${demande.owner ?? ''}`, muted: true },
    {text: `Budget : ${demande.budget ?? 0} €`, muted: false },
    {text: `Description : ${demande.description || 'À compléter'}`, muted: false },
    {text: `Impact : ${demande.impact || 'À compléter'}`, muted: false },
    {text: `Statut : ${demande.status || 'Recue'}`, muted: false },
    ];

    rows.forEach(({text, muted}) => {
              const paragraph = document.createElement('p');
    if (muted) {
        paragraph.className = 'muted';
              }
    paragraph.textContent = text;
    item.appendChild(paragraph);
            });

    list.appendChild(item);
          });
        } catch (error) {
        console.error(error);
    list.innerHTML = '';
    const message = document.createElement('p');
    message.className = 'muted';
    message.textContent = 'Impossible de charger les demandes.';
    list.appendChild(message);
        }
      }

      form.addEventListener('submit', async (event) => {
        event.preventDefault();
    clearMessage();

    const payload = Object.fromEntries(new FormData(form).entries());
    payload.budget = Number(payload.budget);

    try {
          const response = await fetch('/api/demandes', {
        method: 'POST',
    headers: {'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
          });

    if (!response.ok) {
        // Sans ce else, un 400 était avalé en silence et le bouton semblait inerte.
        let details = null;
    try {
        details = describeValidationErrors(await response.json());
            } catch {
        details = null;
            }
    showError(details
    ? `Ajout refusé (erreur ${response.status}) — ${details}`
    : `Ajout refusé (erreur ${response.status}).`);
    return;
          }

    form.reset();
    await loadDemandes();
    showSuccess('Demande ajoutée.');
        } catch (error) {
        console.error(error);
    showError("Impossible de contacter le serveur. Vérifiez qu'il est démarré, puis réessayez.");
        }
      });

    loadDemandes();
